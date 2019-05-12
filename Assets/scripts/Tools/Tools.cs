using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public EToolsType ToolType;
    public delegate void ActionToolHit(EToolsType ToolType,GameObject Go);
    public static event ActionToolHit ToolHitEvent;
   

    public void ToolHit(EToolsType ToolType, GameObject Go)
    {
        ToolHitEvent(ToolType,Go);
    }


    public delegate void ActionDestroyObstacle(GameObject Go);
    public static event ActionDestroyObstacle DestroyOstacleEvent;


    public void DestroyObstacle(GameObject Go)
    {
        DestroyOstacleEvent(Go);
    }

}
