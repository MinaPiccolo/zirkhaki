using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Revy.Framework
{
    [ExecuteInEditMode]
    public class MCalculateCompileTime : MonoBehaviour
    {
#if UNITY_EDITOR
        double startCompileTime = 0;
        bool isCalculating = false;

        // Update is called once per frame
        void Update()
        {

            if (EditorApplication.isCompiling && isCalculating == false)
            {
                isCalculating = true;
                startCompileTime = EditorApplication.timeSinceStartup;
            }

            if (EditorApplication.isCompiling == false && isCalculating == true && startCompileTime > 0)
            {
                isCalculating = false;
                Debug.Log($"Compile Takes {EditorApplication.timeSinceStartup - startCompileTime} seconds.");
                startCompileTime = 0;
            }
        }

#endif
    }
}