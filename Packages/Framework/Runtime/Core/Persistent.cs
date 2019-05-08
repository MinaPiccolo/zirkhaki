using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Revy.Framework
{
    public static class Persistent
    {

        private static GameObject _persistentGameObject;

        public static GameObject PersistentGameObject
        {
            get
            {
                if (_persistentGameObject == null)
                    CreatePersistentGameObject();
                return _persistentGameObject;
            }
        }

        public static void CreatePersistentGameObject()
        {
            if (_persistentGameObject == null)
                _persistentGameObject = new GameObject("Persistent");
        }

        public static void MakePersist(GameObject gameObject)
        {
            if (gameObject != null)
                gameObject.transform.SetParent(PersistentGameObject.transform);
        }

        /// <summary>
        /// Persist component's game object and make it child of game object named 'parentName'.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="parentName"></param>
        public static void MakePersist(FComponent component, string parentName)
        {
            //Find parent game object
            UnityEngine.Transform parentTransform = PersistentGameObject.transform.Find(parentName);

            if (parentTransform == null)
            {
                parentTransform = new UnityEngine.GameObject(parentName).transform;

                parentTransform.SetParent(PersistentGameObject.transform);
            }

            component.transform.SetParent(parentTransform);
        }

        /// <summary>
        /// Persist component's game object and make it child of game object named 'parentName'.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="parentName"></param>
        public static void MakePersist(UnityEngine.GameObject gameObject, string parentName)
        {
            //Find parent game object
            UnityEngine.Transform parentTransform = PersistentGameObject.transform.Find(parentName);

            if (parentTransform == null)
            {
                parentTransform = new UnityEngine.GameObject(parentName).transform;

                parentTransform.SetParent(PersistentGameObject.transform);
            }

            gameObject.transform.SetParent(parentTransform);
        }

        /// <summary>
        /// Persist component's game object and make it child of game object named 'parentName'.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="parentName"></param>
        public static void MakePersist(UnityEngine.Component gameObject, string parentName)
        {
            //Find parent game object
            UnityEngine.Transform parentTransform = PersistentGameObject.transform.Find(parentName);

            if (parentTransform == null)
            {
                parentTransform = new UnityEngine.GameObject(parentName).transform;

                parentTransform.SetParent(PersistentGameObject.transform);
            }

            gameObject.transform.SetParent(parentTransform);
        }

        /// <summary>
        /// Create a game object then add TComponenet to it and make it Persistent sub child.
        /// </summary>
        /// <typeparam name="TComponent">Inherited from FComponent</typeparam>
        ///<param name="gameObjectName">Name of the game object that TComponent will add to it. By default it will set to TComponent full name.</param>
        ///<param name="subcategory">Name of parent game object. By default it will going to "Other" subcategory.</param>
        /// <returns>The instantiated component.</returns>
        public static TComponent Instantiate<TComponent>(string gameObjectName = null,
            string subcategory = null) where TComponent : FComponent
        {
            if (string.IsNullOrEmpty(gameObjectName)) gameObjectName = typeof(TComponent).ToString();

            GameObject persistentChildObject = new GameObject(gameObjectName, typeof(TComponent));

            if (string.IsNullOrEmpty(subcategory)) subcategory = PersistentSubCategories.OTHER;

            Persistent.MakePersist(persistentChildObject, subcategory);

            return persistentChildObject.GetComponent<TComponent>();
        }

        /// <summary>
        /// Clone an object from original object and make it Persistent sub child.
        /// </summary>
        ///<param name="gameObjectName">Instantiated object's name. By default it set to TComponenet full name.</param>
        ///<param name="subcategory">The subcategory of persistent object. By default it will going to Other subcategory.</param>
        /// <returns>The instantiated GameObject.</returns>
        public static GameObject Instantiate(GameObject original, string subcategory = null)
        {
            if (original == null) return null;

            if (string.IsNullOrEmpty(subcategory)) subcategory = PersistentSubCategories.OTHER;

            GameObject persistentChildObject = Object.Instantiate(original);

            Persistent.MakePersist(persistentChildObject, subcategory);

            return persistentChildObject;
        }

        /// <summary>
        /// Clone a object from original object and make it Persistent sub child.
        /// </summary>
        /// <typeparam name="TComponent">Inherited from UnityEngine.Componenet</typeparam>
        ///<param name="gameObjectName">Instantiated object's name. By default it set to TComponenet full name.</param>
        ///<param name="subcategory">The subcategory of persistent object. By default it will going to Other subcategory.</param>
        /// <returns>The instantiated component.</returns>
        public static TComponenet Instantiate<TComponenet>(TComponenet original, string subcategory = null)
            where TComponenet : Component
        {
            if (original == null) return default(TComponenet);

            TComponenet persistentChildObject = Object.Instantiate(original);

            Persistent.MakePersist(persistentChildObject, subcategory);

            return persistentChildObject;
        }

        /// <summary>
        /// Add component to subcategory game object and make it persistent.
        /// </summary>
        ///<param name="subcategory">The subcategory of persistent object. By default it will going to Other subcategory.</param>
        /// <returns>The instantiated component.</returns>
        public static Component Instantiate(Type component, string subcategory = null,
            string parentName = null)
        {
            if (component == null) return null;

            if (string.IsNullOrEmpty(subcategory)) subcategory = PersistentSubCategories.OTHER;
            if (string.IsNullOrEmpty(parentName)) parentName = subcategory;

            var subcategoryGo = new GameObject(subcategory);

            Component result = subcategoryGo.AddComponent(component);

            Persistent.MakePersist(subcategoryGo, parentName);

            return result;
        }
    }
}