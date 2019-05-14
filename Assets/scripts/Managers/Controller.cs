 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject HUDGo;
    //public Camera CamGame;
    public static Controller Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        if (PlayerPrefs.GetInt("intVar") == 0)
        {
            PlayerPrefs.SetInt("intVar", 1);
        }
    }
    public void InitialMainGame()
    {
        HUDGo.SetActive(true);
        ToolController.Instance.InitialHUDLvl();
    }

    void Update()
    {
        
    }
    public GameObject generateGameObj(Transform prefab,Transform parent)
    {
        GameObject go = SimplePool.Spawn(prefab.gameObject, Vector3.zero, Quaternion.identity); 
        go.transform.SetParent(parent, prefab);
        go.transform.position = prefab.position;
        return go;
    }
}
