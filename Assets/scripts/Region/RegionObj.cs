using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionObj : MonoBehaviour
{
    public RegionData RegionInfoObj;
    void Start()
    {
    }

    void OnMouseDown()///2 bar seda zade mishe???????????????
    {
       // if (gameObject == RegionController.Instance.RegionGo[RegionInfoObj.RegionId])
        {
            ObstacleController.Instance.generateObstacle(RegionInfoObj.RegionId);
            transform.parent.gameObject.SetActive(false);
        }
    }
    void Update()
    {

    }
}
