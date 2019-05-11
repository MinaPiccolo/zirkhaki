using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject[] itemsGo;
    public Transform parentItems;

    public static ItemController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
   
    void Start()
    {

    }

    public void GenerateItem(Vector2 ObstaclePos)
    {
        ItemsData itemList = LoadData.Instance.LevelInfo[LvlController.Instance.CurrentLevelIndex].ItemsList;
        
        GameObject item = Controller.Instance.generateGameObj(itemList.ItemLocation, parentItems);
        //ObstacleGo.Add(obstacle);
        Vector2 pos = ObstaclePos;
        pos.y += 1;
        item.transform.position = pos;
        ItemObj script = item.AddComponent<ItemObj>();
        script.ItemInfoObj = itemList;


        //GameObject item = Controller.Instance.generateGameObj(itemsGo[itemObjIndex], parentItems);
        //item.transform.position = new Vector2(pos.x, pos.y + 1);

        //item.GetComponent<ItemObj>().ItemId = itemObjIndex;
        //item.GetComponent<ItemObj>().ItemType = EItemType.Junk;
        //item.GetComponent<ItemObj>().ItemName = "";
        //item.GetComponent<ItemObj>().PiecesCount = 4;
    }
   
    void Update()
    {
        
    }
}
