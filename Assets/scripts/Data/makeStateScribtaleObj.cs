using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class makeStateScribtaleObj 
{


    [MenuItem("zirkhaki/Scriptable Objects/Map Data")]
    public static void CreateMyAsset()
    {
        var asset = ScriptableObject.CreateInstance<MapData>();

        AssetDatabase.CreateAsset(asset, "Assets/scriptableObjects/mapData.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

   // [MenuItem("zirkhaki/Scriptable Objects/City Data")]
    //public static void CreateMyAsset1()
    //{
    //    var asset = ScriptableObject.CreateInstance<CityData>();

    //    AssetDatabase.CreateAsset(asset, "Assets/scriptableObjects/cityData.asset");
    //    AssetDatabase.SaveAssets();

    //    EditorUtility.FocusProjectWindow();

    //    Selection.activeObject = asset;
    //}
}
