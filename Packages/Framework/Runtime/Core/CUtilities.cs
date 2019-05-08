/*
 * General utilities class.
 * Author: Ideen Molavi Nejad. ideenmolavi@gmail.com
 */

using System.Reflection;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Revy.Framework
{
    public static class CUtilities
    {
        private static bool _isValidEmail;

        /// <summary>
        /// Is used in GetAllTypes method to cache result.
        /// </summary>
        private static List<Type> _allTypesCache = null;

        /// <summary>
        /// Binding flags that is used to find properties.
        /// </summary>
        public static readonly BindingFlags PROPERTIES_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        #region Public Static Methods

        /// <summary>
        /// Set a property by reflection and use CUtilities.PROPERTIES_BINDING_FLAGS bindings as default bindings flags.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="bindigFlag"></param>
        /// <returns></returns>
        public static bool SetPropertyByReflection(string propertyName, System.Object obj, System.Object value, BindingFlags? bindigFlag = null)
        {
            if (obj == null)
            {
                Debug.LogWarning("Can not set property by reflection because target object is null");
                return false;
            }

            BindingFlags tmpBindingFlags = bindigFlag != null ? (BindingFlags)bindigFlag : PROPERTIES_BINDING_FLAGS;
            var pi = obj.GetType().GetProperty(propertyName, tmpBindingFlags);

            if (pi == null)
            {
                Debug.LogWarningFormat("Can not set a {0} property by reflection because the property was not exist in {1}.", propertyName, obj.GetType().FullName);
                return false;
            }

            return SetProperty(pi, obj, value, tmpBindingFlags);
        }

        /// <summary>
        /// Set a property by reflection and use CUtilities.PROPERTIES_BINDING_FLAGS bindings as default bindings flags.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="bindigFlag"></param>
        /// <returns></returns>
        public static bool SetPropertyByReflection(PropertyInfo propertyInfo, System.Object obj, System.Object value, BindingFlags? bindigFlag = null)
        {
            if (obj == null)
            {
                Debug.LogWarning("Can not set property by reflection because target object is null");
                return false;
            }

            if (propertyInfo == null)
            {
                Debug.LogWarningFormat("Can not set property by reflection on {0} because property info is null.");
            }

            BindingFlags tmpBindingFlags = bindigFlag != null ? (BindingFlags)bindigFlag : PROPERTIES_BINDING_FLAGS;

            return SetProperty(propertyInfo, obj, value, tmpBindingFlags);
        }

        /// <summary>
        /// Set a property by reflection and use CUtilities.PROPERTIES_BINDING_FLAGS bindings as default bindings flags.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        /// <param name="bindigFlag"></param>
        /// <returns></returns>
        public static bool SetFieldByReflection(string fieldName, System.Object obj, System.Object value, BindingFlags? bindigFlag = null)
        {
            if (obj == null)
            {
                Debug.LogWarningFormat("Can not set field by reflection because target object is null.");
                return false;
            }

            if (string.IsNullOrEmpty(fieldName))
            {
                Debug.LogWarningFormat("Can not set field by reflection because field name is null or empty.");
                return false;
            }

            try
            {
                BindingFlags tmpBindingFlags = bindigFlag != null ? (BindingFlags)bindigFlag : PROPERTIES_BINDING_FLAGS;

                Type objectType = obj.GetType();
                FieldInfo fieldInfo = objectType.GetField(fieldName, tmpBindingFlags);

                if (fieldInfo == null)
                {
                    Debug.LogWarningFormat("Can not set field by reflection because {0} field is not exist in {1}.", fieldName, objectType.FullName);
                    return false;
                }

                fieldInfo.SetValue(obj, value);
            }
            catch (Exception e)
            {

                Debug.LogException(e);
                return false;
            }

            return true;
        }

        public static bool SetFieldByReflection(FieldInfo fieldInfo, System.Object obj, System.Object value, BindingFlags? bindigFlag = null)
        {
            if (obj == null)
            {
                Debug.LogWarningFormat("Can not set field by reflection because target object is null.");
                return false;
            }

            if (fieldInfo == null)
            {
                Debug.LogWarningFormat("Can not set field by reflection because field info is null.");
                return false;
            }

            try
            {
                fieldInfo.SetValue(obj, value);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns 'Assembly-CSharp' assembly.
        /// </summary>
        public static Assembly GetAssembly()
        {
            return Assembly.Load(new AssemblyName("Assembly-CSharp"));
        }

        public static Type[] GetAllDerivedTypes(Type targetType)
        {
            var result = new List<Type>();
            var currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsSubclassOf(targetType))
                        result.Add(type);
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Return all types that implement interfaceType.
        /// </summary>
        /// <param name="interfaceType">Type of interface</param>
        /// <param name="includeAbstract">invclude abstract classes in final result?</param>
        /// <returns></returns>
        public static Type[] GetAllImplementingTypes(Type interfaceType, bool includeAbstract = false)
        {
            if (interfaceType.IsInterface == false) return null;
            var result = new List<Type>();
            var currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsInterface) continue;
                    if (!type.GetInterfaces().Contains(interfaceType)) continue;
                    if (!includeAbstract && type.IsAbstract) continue;
                    result.Add(type);
                }
            }
            return result.ToArray();
        }

        public static Type[] GetAllTypes()
        {
            if (_allTypesCache != null) return _allTypesCache.ToArray();

            var currentDomain = AppDomain.CurrentDomain;
            var assemblies = currentDomain.GetAssemblies();
            _allTypesCache = new List<Type>(22000);
            foreach (var assembly in assemblies)
            {
                _allTypesCache.AddRange(assembly.GetTypes());
            }
            return _allTypesCache.ToArray();
        }

        public static FieldInfo[] GetAllFieldWithAttribute(Type type, Type attribute)
        {
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            return type.GetFields(bindingFlags).Where(field => field.IsDefined(attribute, true)).ToArray();
        }

        public static FieldInfo[] GetAllFieldInAssemblyWithAttribute(Type attribute)
        {
            Assembly assembly = GetAssembly();

            Type[] assemblyTypes = assembly.GetTypes();

            List<FieldInfo> result = new List<FieldInfo>();

            foreach (Type it in assemblyTypes)
            {
                FieldInfo[] fieldInfos = GetAllFieldWithAttribute(it, attribute);

                result.AddRange(fieldInfos);
            }

            return result.ToArray();
        }

        public static string GetPlatform()
        {
#if UNITY_EDITOR
            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.iOS:
                    return "ios";
                case BuildTarget.Android:
                    return "android";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "windowsstandalone";
                default:
                    return "android";
            }
#endif

#if !UNITY_EDITOR
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return "ios";
                case RuntimePlatform.Android:
                    return "android";
                case RuntimePlatform.WindowsPlayer:
                    return "windowsstandalone";
                default:
                    return "android";
            }
#endif
        }

        /// <summary>
        /// Return normalize percent value.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float GetPercent(float target, float max)
        {
            return Mathf.Clamp(((100f * (target)) / max) / 100f, 0f, 1f);
        }

        public static bool IsValidEmail(string strIn)
        {
            _isValidEmail = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (_isValidEmail)
                return false;

            // Return true if strIn is in valid email format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsPhoneNumber(string number)
        {
            return Regex.Match(number, @"^(\+[0-9]{9})$").Success;
        }

        public static bool IsMobilePhoneNumber(string number)
        {
            return Regex.Match(number, @"^9\d{9}$").Success;
        }

        public static float Map(float a1, float a2, float b1, float b2, float s)
        {
            float value = b1 + (s - a1) * (b2 - b1) / (a2 - a1);
            return Mathf.Clamp(value, b1, b2);
        }
        #endregion

        #region Helpers
        private static bool SetProperty(PropertyInfo propertyInfo, System.Object obj, System.Object value, BindingFlags bindigFlag)
        {
            if (obj == null) return false;
            if (propertyInfo == null) return false;

            try
            {
                if (propertyInfo.CanWrite)
                {
                    propertyInfo.SetValue(obj, value);
                }
                else
                {
                    var declaringType = propertyInfo.DeclaringType;
                    var declaringTypeProperty = declaringType.GetProperty(propertyInfo.Name, bindigFlag);

                    if (declaringTypeProperty == null) return false;

                    declaringTypeProperty.SetValue(obj, value);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return false;
            }
            return true;
        }

        private static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                _isValidEmail = true;
            }
            return match.Groups[1].Value + domainName;
        }
        #endregion
    }
}