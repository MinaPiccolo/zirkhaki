using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ObstacleData
{
    public int ObstacleId;
    public Transform LocationObstacle;
    public EObstacleType TypeObstacle;
    public int NumberScrachToDestroy;
}
