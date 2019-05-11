using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionController : MonoBehaviour
{
    public Transform RegionParent;
    //public List<GameObject> RegionGo;
    public static RegionController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Debug.Log(LoadData.Instance.RegionInfo[0].);
        //   waiteFrameForEnable();
        StartCoroutine(waiteFrameForEnable());
    }
    IEnumerator waiteFrameForEnable()
    {
        yield return new WaitForSeconds(0.5f);
        generateRegion();
    }

    void generateRegion()
    {
       // RegionGo.Clear();
        for (int i = 0; i < LoadData.Instance.RegionInfo.Count; i++)
        {
            GameObject region = Controller.Instance.generateGameObj(LoadData.Instance.RegionInfo[i].LocationRegion, RegionParent);
            RegionObj script = region.AddComponent<RegionObj>();
            script.RegionInfoObj = LoadData.Instance.RegionInfo[i];

           // RegionGo.Add(region);
        }
    }
}
