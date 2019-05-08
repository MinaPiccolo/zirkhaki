using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Revy.Framework
{
    public class CServiceLocator
    {

        #region Fields
        private const string LOG_TAG = "Service Locator";
        #endregion

        #region Properties

        private static Dictionary<Type, System.Object> _services = new Dictionary<Type, System.Object>();

        #endregion

        #region Public Methods

        /// <summary>
        /// Register a service into Service Locator.
        /// serviceType parameter must be an interface type.
        /// serviceInstance parameter must implement serviceType interface.
        /// </summary>
        /// <param name="serviceType">Must be an interface.</param>
        /// <param name="serviceInstance">Concrete class instance for serviceType interface.</param>
        public static void Register(Type serviceType, System.Object serviceInstance)
        {
            if (serviceInstance != null && serviceType != null)
            {
                if (IsValidRegistration(serviceType, serviceInstance) == false) return;

                if (_services.ContainsKey(serviceType) == false)
                    _services.Add(serviceType, serviceInstance);
                else
                {
                    Debug.LogWarningFormat("Type of '{0}' is already exist in Service Locator.", serviceType.Name);
                }
            }
            else if (serviceInstance == null && serviceType != null)
            {
                Debug.LogWarningFormat(
                    "Registering {0} in service locater is failed because service instance is null.",
                    serviceType.Name);

            }
            else if (serviceInstance != null && serviceType == null)
            {
                Debug.LogWarningFormat(
                    "Registering {0} in service locater is failed because service type is null.",
                    serviceInstance.GetType().Name);
            }
            else if (serviceInstance == null && serviceType == null)
            {
                Debug.LogWarningFormat(
                    "Registering service in service locater is failed because service type and service instance is null.");
            }
        }

        public static System.Object Resolve(Type serviceType)
        {
            System.Object service = null;
            try
            {
                service = _services[serviceType];
                if (service == null)
                {
                    _services.Remove(serviceType);
                    throw new KeyNotFoundException();
                }
            }
            catch (KeyNotFoundException)
            {
                CLog.Warning($"Service of type '{serviceType}' not found in Service Locator.", category: LOG_TAG);
            }
            catch (ArgumentNullException)
            {
                CLog.Warning("Null service type passed to Service Locator. This is not accepted behavior.", category: LOG_TAG);
            }
            return service;
        }

        public static void UnRegister(Type serviceType)
        {
            if (serviceType != null)
                _services.Remove(serviceType);
        }
        #endregion

        #region Helpers

        /// <summary>
        /// Check registration is valid.
        /// Generate warning when validation is not valid.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="serviceInstance"></param>
        /// <returns></returns>
        private static bool IsValidRegistration(Type serviceType, object serviceInstance)
        {
            if (serviceType.IsInterface == false)
            {
                Debug.LogError(
                    "Service type is not an interface, you should register interface instead of concrete classes for service type argument.");
                return false;
            }
            if (serviceInstance.GetType().GetInterfaces().Contains(serviceType) == false)
            {
                Debug.LogErrorFormat(
                    "Service instance({0}) is not implementing right interface ({1}).", serviceInstance.GetType().Name, serviceType.Name);
                return false;
            }
            return true;
        }

        #endregion

    }
}