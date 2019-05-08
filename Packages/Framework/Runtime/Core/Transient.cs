using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Revy.Framework
{
    public static class Transient
    {
        private static GameObject _transientGameObject;

        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static GameObject TransientGameObject
        {
            get
            {
                if (_transientGameObject == null)
                    CreateTransientGameObject();
                return _transientGameObject;
            }
        }
        
        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static void MakeTransient(GameObject obj)
        {
            if (obj != null)
                obj.transform.SetParent(TransientGameObject.transform);
        }

        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static void CreateTransientGameObject()
        {
            if (_transientGameObject == null)
                _transientGameObject = new GameObject("Transient");
        }

        
        /// <summary>
        /// Create a game object then add TBehaviour component to it and make it transient.
        /// </summary>
        /// <typeparam name="TComponent">Inherited from FComponent</typeparam>
        /// <param name="gameObjectName">Instantiated object's name. By default it set to TComponenet full name.</param>
        /// <returns>The instantiated component.</returns>
        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static TComponent Instantiate<TComponent>(string gameObjectName) where TComponent : FComponent
        {
            if (string.IsNullOrEmpty(gameObjectName)) gameObjectName = typeof(TComponent).ToString();

            GameObject transientChildObject =
                new GameObject(gameObjectName, typeof(TComponent));

            transientChildObject.transform.SetParent(Transient.TransientGameObject.transform);

            return transientChildObject.GetComponent<TComponent>();
        }

        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static TComponent Instantiate<TComponent>(TComponent original) where TComponent : Component
        {
            TComponent transientChildObject = Object.Instantiate(original);

            transientChildObject.transform.SetParent(Transient.TransientGameObject.transform);

            return transientChildObject;
        }

        /// <summary>
        /// Clone an object from original object and make it transient.
        /// </summary>
        /// <returns>The instantiated GameObject.</returns>
        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static GameObject Instantiate(FComponent original)
        {
            if (original == null) return null;
            return Object.Instantiate(original.gameObject, Transient.TransientGameObject.transform);
        }

        /// <summary>
        /// Clone an object from original object and make it transient.
        /// </summary>
        /// <returns>The instantiated GameObject.</returns>
        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static GameObject Instantiate(GameObject original)
        {
            if (original == null) return null;
            return Object.Instantiate(original, Transient.TransientGameObject.transform);
        }

        /// <summary>
        /// Instantiate a game object and attach component to it.
        /// Component's game object can have a parent game object if  'parentGameObjectName' is not null or empty.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="gameObjectName"></param>
        /// <param name="parentGameObjectName"></param>
        /// <returns></returns>
        [Obsolete("Transient concept has been removed from the Framework. Objects that are not Persistent considered Transient.")]
        public static Component Instantiate(Type component, string gameObjectName,
            string parentGameObjectName = null)
        {
            if (component == null || string.IsNullOrEmpty(gameObjectName))
            {
                CLog.Warning("Can not instantiate transient object because arguments are not valid!");
                return null;
            }

            if (_transientGameObject == null)
            {
                CLog.Warning(
                    "Can not instantiate transient object because 'MFramework.TransientGameObject' is null!");
                return null;
            }

            GameObject componentGo = new GameObject(gameObjectName);
            Component result = componentGo.AddComponent(component);
            Transform transientTransform = _transientGameObject.transform;

            if (!string.IsNullOrEmpty(parentGameObjectName))
            {
                Transform currentParent = null;
                Transform existingParent = transientTransform.Find(parentGameObjectName);

                if (existingParent == null)
                    currentParent = new GameObject(parentGameObjectName).transform;
                else
                    currentParent = existingParent;

                currentParent.SetParent(transientTransform);
                componentGo.transform.SetParent(currentParent);
            }
            else
                componentGo.transform.SetParent(transientTransform);

            return result;
        }
    }
}