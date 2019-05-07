using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct RegionData
{
    public int RegionId;
    public Transform LocationRegion;
    public string SpriteRegionImg;
    public List<ObstacleData> ObstaclesList;
}
