using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ItemsData
{
    public List<MainItemsData> MainItemsList;
    public List<RareItemsData> RareItemsList;
    public int NumberJunkItems;
    public Transform ItemLocation;

}
