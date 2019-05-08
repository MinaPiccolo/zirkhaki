/*
 * An injection is the passing of a dependency to a dependent object (a client) that would use it.
 * Use CInjectAttribute with property or field to inject dependency.
 * You need to invoke CDependencyInjection.Inject() method on instance of target class.
 * This class tightly coupled with CServiceLocator class.
 * Author: Ideen Molavi Nejad, IdeenMolavi@gmail.com
 * Creation Time: 08-09-2017
 * 
*/
using System;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace Revy.Framework
{
    /// <summary>
    /// An injection is the passing of a dependency to a dependent object (a client) that would use it.
    /// Use CInjectAttribute with property or field to inject dependency.
    /// You need to invoke CDependencyInjection.Inject() method on instance of dependent class.
    /// This class tightly coupled with CServiceLocator class.
    /// </summary>
    public class CDependencyInjection
    {
        #region Fields

        /// <summary>
        /// Binding flags that is used to find properties.
        /// </summary>
        private const BindingFlags PROPERTIES_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        /// <summary>
        /// Binding flags that is used to find fields.
        /// </summary>
        private const BindingFlags FIELLDS_BINDING_FLAGS = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        #endregion

        #region Public Methods

        /// <summary>
        /// Inject dependencies into the dependence class.
        /// </summary>
        /// <param name="dependence"></param>
        public static void Inject(System.Object dependence, Type type = null)
        {
            if (dependence == null)
            {
                Debug.LogWarning("Dependency injection encounter null argument.This in not accepted behavior.");
                return;
            }

            InjectFields(dependence, type);
            InjectProperties(dependence, type);
        }

        #endregion Public Methods

        #region Helpers

        /// <summary>
        /// Search through properties of dependence class and inject dependencies.
        /// </summary>
        /// <param name="dependence"></param>
        private static void InjectProperties(object dependence, Type type = null)
        {
            if (dependence == null) return;

            Type dependenceType = dependence.GetType();
            PropertyInfo[] propertiesInfoes =
                dependenceType.GetProperties(PROPERTIES_BINDING_FLAGS);

            var propertiesCount = propertiesInfoes.Length;

            for (int i = 0; i < propertiesCount; i++)
            {
                var propertyInfo = propertiesInfoes[i];
                var hasInjectAttribute = propertyInfo.IsDefined(typeof(CInjectAttribute), false);
                if (hasInjectAttribute == false) continue;

                GenerateWarningWhenTypeIsNotInterface(propertyInfo.PropertyType);

                if (type != null && !propertyInfo.PropertyType.GetInterfaces().Contains(type)) continue;

                var found = CServiceLocator.Resolve(propertyInfo.PropertyType);

                if (found != null)
                {
                    CUtilities.SetPropertyByReflection(propertyInfo, dependence, found, PROPERTIES_BINDING_FLAGS);
                }
            }
        }

        /// <summary>
        /// Search through fields of dependence class and inject dependencies.
        /// </summary>
        /// <param name="dependence"></param>
        private static void InjectFields(object dependence, Type type = null)
        {
            if (dependence == null) return;

            Type dependenceType = dependence.GetType();
            FieldInfo[] fieldsInfo =
                dependenceType.GetFields(FIELLDS_BINDING_FLAGS);

            var fieldsCount = fieldsInfo.Length;
            for (int i = 0; i < fieldsCount; i++)
            {
                var fieldinfo = fieldsInfo[i];
                var hasInjectAttribute = fieldinfo.IsDefined(typeof(CInjectAttribute), false);
                if (hasInjectAttribute == false) continue;

                GenerateWarningWhenTypeIsNotInterface(fieldinfo.FieldType);

                if (type != null && !fieldinfo.FieldType.GetInterfaces().Contains(type)) continue;

                var found = CServiceLocator.Resolve(fieldinfo.FieldType);
                if (found != null)
                    CUtilities.SetFieldByReflection(fieldinfo, dependence, found, FIELLDS_BINDING_FLAGS);
            }
        }

        private static void GenerateWarningWhenTypeIsNotInterface(Type itemType)
        {
            if (itemType.IsInterface == false)
                UnityEngine.Debug.LogWarningFormat("For injection the property's or field's type needs to be an interface but it is not.property or filed name: {0}", itemType.FullName);
        }
    }

    #endregion
}