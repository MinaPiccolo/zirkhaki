using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvlController : MonoBehaviour
{
    public int CurrentLevelIndex;
    public List<LevelData> TempLvlList;

    public ItemsData TempItemInfo;
    public static LvlController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }
    void OnEnable()
    {
        StartCoroutine(waiteFrameForEnable());
    }
    IEnumerator waiteFrameForEnable()
    {
        yield return new WaitForSeconds(0.5f);
        TempItemInfo = LoadData.Instance.StateInfo[0].LevelsList[CurrentLevelIndex].ItemsList;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Return))
        //    TempItemInfo.MainItemsList.RemoveAt(0);
    }
}
