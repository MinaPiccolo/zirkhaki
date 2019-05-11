using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionObj : MonoBehaviour
{
    public RegionData RegionInfoObj;
    void Start()
    {
    }

    void OnMouseDown()
    {
        Controller.Instance.InitialMainGame();
        ObstacleController.Instance.generateObstacle(RegionInfoObj.RegionId);
        transform.parent.gameObject.SetActive(false);
    }
    void Update()
    {

    }
}
