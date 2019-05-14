using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ItemsData
{
    public List<MainItemsData> MainItemsList;
    public List<RareItemsData> RareItemsList;
    public List<JunkItemsData> JunkItemsList;
    public Transform ItemLocation;

}
