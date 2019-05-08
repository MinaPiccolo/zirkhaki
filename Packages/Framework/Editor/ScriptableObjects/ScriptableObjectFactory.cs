using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Revy.Framework;
using System;

namespace Revy.Framework.Editor
{
    /// <summary>
    /// Helper class for instantiating ScriptableObjects.
    /// </summary>
    public class ScriptableObjectFactory
    {
        private static List<Type> _finalItems = new List<Type>();
        private static Type[] allScriptableObjects = null;

        [MenuItem("Revy/Create/ScriptableObject")]
        public static void Create()
        {
            //Get all classes derived from ScriptableObject
            allScriptableObjects = CUtilities.GetAllDerivedTypes(typeof(ScriptableObject));
            try
            {
                foreach (var item in allScriptableObjects)
                {
                    if (item == null) continue;
                    if (item.Namespace == "Unity" || item.Namespace == "UnityEditor" || item.Namespace == "UnityEngine") continue;
                    if (item.Namespace != null && (item.Namespace.Contains("UnityEditor") || 
                                                   item.Namespace.Contains("UnityEngine") ||
                                                   item.Namespace.Contains("JetBrains") ||
                                                   item.Namespace.Contains("Unity")
                                                   )) continue;
                    _finalItems.Add(item);
                }
            }
            catch(Exception)
            {
                Debug.Log("exception");
            }

            Debug.Log("Filtred object count = " + _finalItems.Count);
            //Show the selection window.
            var window = EditorWindow.GetWindow<ScriptableObjectWindow>(true, "Create a new ScriptableObject", true);
            window.ShowPopup();

            window.Types = _finalItems.ToArray();
            _finalItems.Clear();
            allScriptableObjects = null;
        }
    }
}