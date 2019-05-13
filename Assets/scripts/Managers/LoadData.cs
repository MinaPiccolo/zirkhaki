using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class LoadData : MonoBehaviour
{
    public MapData MapInfo;
    public MapData NewMap;
    public List<StateData> StateInfo;
    public List<LevelData> LevelInfo;
    public List<RegionData> RegionInfo;
   // public ItemsData ItemInfo;
    //public List<ObstacleData> ObstacleInfo;
    public static LoadData Instance { get; private set; }

    void Awake()
    {
        Instance = this;
        NewMap = Instantiate(MapInfo);
    }

    void Start()
    {
        StateInfo = NewMap.StatesList;

        foreach (StateData stateData in StateInfo)
        {
            LevelInfo = stateData.LevelsList;
            foreach (LevelData ld in LevelInfo)
            {
                RegionInfo = ld.RegionsList;

                //foreach (RegionData rd in RegionInfo)
                //    ObstacleInfo = rd.ObstaclesList;

            }
            //foreach (LevelData ld in LevelInfo)
            //    ItemInfo = ld.ItemsList;
        }

    }

   
}
