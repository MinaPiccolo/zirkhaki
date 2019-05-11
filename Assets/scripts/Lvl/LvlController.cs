using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlController : MonoBehaviour
{
    public int CurrentLevelIndex;
    public static LvlController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    //public Transform RegionParent;
    //void Start()
    //{
    //    Debug.Log(LoadData.Instance.StateInfo[] LvlInfo.LevelId);
    //    for (int i = 0; i < LvlInfo.RegionsList.Count; i++)
    //    {
    //        GameObject region = Controller.Instance.generateGameObj(LvlInfo.RegionsList[i].LocationRegion.gameObject, LvlInfo.RegionsList[i].LocationRegion.transform);
    //        region.transform.SetParent(RegionParent);
    //        RegionObj scRD= region.AddComponent<RegionObj>();
    //        scRD.RegionInfo = LvlInfo.RegionsList[i];
    //    }
    //}

    void Update()
    {
        
    }
}
