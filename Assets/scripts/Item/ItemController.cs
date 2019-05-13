using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject[] itemsGo;
    public Transform parentItems;
    string mainlist, rarelist, junk;
    //ItemsData ItemList;
    public static ItemController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {

    }

    public void GenerateItem(ItemsData itemList)
    {
        convertListToSt(itemList);
        GameObject item = Controller.Instance.generateGameObj(itemList.ItemLocation, parentItems);
        Vector2 pos = itemList.ItemLocation.position;
        pos.y += 2;
        item.transform.position = pos;

        GenerateItemRandomDesignWithLastMainPiece(itemList, item);
    }
    void GenerateItemRandomDesignWithLastMainPiece(ItemsData itemList, GameObject item)
    {
        if (CheckLastPiecesMainItem(itemList))
        {
            if (itemList.NumberJunkItems > 0 && itemList.RareItemsList.Count > 0)
            {
                int ranTypeItem = Random.Range(0, 100);
                if (ranTypeItem < 50)
                    GenerateJunkItem(item, EItemType.Junk, itemList);
                else
                    GenerateRareItem(item, EItemType.Rare, itemList);
            }
            else if (itemList.NumberJunkItems > 0 && itemList.RareItemsList.Count == 0)
                GenerateJunkItem(item, EItemType.Junk, itemList);
            else if (itemList.NumberJunkItems == 0 && itemList.RareItemsList.Count > 0)
                GenerateRareItem(item, EItemType.Rare, itemList);
            else
                GenerateMainItem(item, EItemType.Main, itemList);
        }
        else
        {
            int ranTypeItem = Random.Range(0, 90);
            if (ranTypeItem < 30)
                GenerateJunkItem(item, EItemType.Junk, itemList);
            else if (ranTypeItem >= 30 && ranTypeItem < 60)
                GenerateRareItem(item, EItemType.Rare, itemList);
            else
                GenerateMainItem(item, EItemType.Main, itemList);
        }
    }

    bool CheckLastPiecesMainItem(ItemsData itemList)
    {
        if (itemList.MainItemsList.Count == 1)
            if (itemList.MainItemsList[0].NumberOfFound + 1 >= itemList.MainItemsList[0].NumberOfPices)
                return true;
        return false;
    }
    void GenerateMainItem(GameObject item, EItemType type, ItemsData itemList)
    {
        ItemObj itemObjScr = item.AddComponent<ItemObj>();
        //script.ItemInfoObj = itemList;
        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.MainItemsList.Count);
        itemObjScr.ItemId = ranId;
        itemObjScr.MainItemsInfo = itemList.MainItemsList[ranId];
        itemList.MainItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    void GenerateRareItem(GameObject item, EItemType type, ItemsData itemList)
    {
        ItemObj itemObjScr = item.AddComponent<ItemObj>();
        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.RareItemsList.Count);
        itemObjScr.ItemId = ranId;
        itemObjScr.RareItemsInfo = itemList.RareItemsList[ranId];
        itemList.MainItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    void GenerateJunkItem(GameObject item, EItemType type, ItemsData itemList)
    {
        ItemObj itemObjScr = item.AddComponent<ItemObj>();
        itemObjScr.ItemType = type;
        itemObjScr.ItemId = itemList.NumberJunkItems;
        itemList.NumberJunkItems--;
        convertListToSt(itemList);
    }

    void Update()
    {

    }
    void convertListToSt(ItemsData itemList)
    {
        mainlist = "";
        rarelist = "";
        for (int i = 0; i < itemList.MainItemsList.Count; i++)
            mainlist += itemList.MainItemsList[i].NameMainItem + ",";

        for (int i = 0; i < itemList.RareItemsList.Count; i++)
            rarelist += itemList.RareItemsList[i].NameRareItem + ",";

        junk = itemList.NumberJunkItems + ",";

        
    }
    void OnGUI()
    {
        GUI.color = Color.red;
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = Color.red;
        myStyle.fontSize = 100;
        GUI.Label(new Rect(10, 0, 400, 100), mainlist + " ml", myStyle);
        GUI.Label(new Rect(10, 100, 400, 100), rarelist + " rl", myStyle);
        GUI.Label(new Rect(10, 200, 400, 100), junk + " j", myStyle);

    }
}
