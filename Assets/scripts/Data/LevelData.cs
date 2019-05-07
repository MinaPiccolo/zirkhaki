using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct LevelData 
{
    public int LevelId;
    public string NameLvl;
    public string Description;
    public string AddressImg;
    public Transform LocationLvl;
    public int Cost;
    public float timeLvl;
    public List<RegionData> RegionsList;
    public ItemsData ItemsList;
}
