using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    static controller _instance;
    public static controller Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("intVar") == 0)
        {
            PlayerPrefs.SetInt("intVar", 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject generateGameObj(GameObject prebab,Transform parent)
    {
        GameObject go = simplePool.Spawn(prebab, Vector3.zero, Quaternion.identity);
        go.transform.SetParent(parent, false);
        go.transform.localPosition = Vector3.zero;
        return go;
    }
}
