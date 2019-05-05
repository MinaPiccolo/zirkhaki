using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controllerMenu : MonoBehaviour
{
   

    //public int CurrentState
    //{
    //    set { currentState = value; }
    //    get { return currentState; }
    //}
    static controllerMenu _instance;
    public static controllerMenu Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

  
   

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
