using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stateBtn : MonoBehaviour
{
    public int indexState;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { onClickStateBtn(); });
    }
    void onClickStateBtn()
    {
        mapMoving.Instance.currentState = indexState;
        mapMoving.Instance.zoomin();
    }
    void Update()
    {
        
    }
}
