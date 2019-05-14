using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject[] itemsGo;
    public Transform parentItems;
    string mainlist, rarelist, junkList;
    //ItemsData ItemList;
    public static ItemController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        convertListToSt(LvlController.Instance.TempItemInfo);
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
        Debug.Log(CheckLastPiecesMainItem(itemList));
        if (CheckLastPiecesMainItem(itemList))
        {
            if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count > 0)
                RandomGenerateItem(item, EItemType.Junk, EItemType.Rare, itemList);
            else if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count == 0)
                RandomGenerateItem(item, EItemType.Junk, itemList);
            else if (itemList.JunkItemsList.Count == 0 && itemList.RareItemsList.Count > 0)
                RandomGenerateItem(item, EItemType.Rare, itemList);
            else
                RandomGenerateItem(item, EItemType.Main, itemList);
        }
        else
        {
            Debug.Log("ordinary");
            if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count > 0)
            {
                Debug.Log("1");
                RandomGenerateItem(item, EItemType.Junk, EItemType.Rare, EItemType.Main, itemList);
            }
            else if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count == 0 )
            {
                Debug.Log("2");
                RandomGenerateItem(item, EItemType.Junk, EItemType.Main, itemList);
            }
            else if (itemList.JunkItemsList.Count == 0 && itemList.RareItemsList.Count > 0)
            {
                Debug.Log("3");
                RandomGenerateItem(item, EItemType.Rare, EItemType.Main, itemList);
            }
            else if (itemList.JunkItemsList.Count == 0 && itemList.RareItemsList.Count == 0)
            {
                Debug.Log("4");
                RandomGenerateItem(item,EItemType.Main, itemList);
            }
        }
    }

    void RandomGenerateItem(GameObject item, EItemType type1, ItemsData itemList)
    {
        GenerateItemBaseOnType(item, type1, itemList);
    }
    void RandomGenerateItem(GameObject item, EItemType type1, EItemType type2, ItemsData itemList)
    {
        int ranTypeItem = Random.Range(0, 100);
        if (ranTypeItem < 50)
            GenerateItemBaseOnType(item, type1, itemList);
        else
            GenerateItemBaseOnType(item, type2, itemList);
    }
    void RandomGenerateItem(GameObject item, EItemType type1, EItemType type2, EItemType type3, ItemsData itemList)
    {
        int ranTypeItem = Random.Range(0, 90);
        if (ranTypeItem < 30)
            GenerateItemBaseOnType(item, type1, itemList);
        else if (ranTypeItem >= 30 && ranTypeItem < 60)
            GenerateItemBaseOnType(item, type2, itemList);
        else
            GenerateItemBaseOnType(item, type3, itemList);
    }
   void GenerateItemBaseOnType(GameObject item,EItemType type, ItemsData itemList)
    {
        if (type == EItemType.Main)
            GenerateMainItem(item, EItemType.Main, itemList);
        else if (type == EItemType.Rare)
            GenerateRareItem(item, EItemType.Rare, itemList);
        else if (type == EItemType.Junk)
            GenerateJunkItem(item, EItemType.Junk,itemList);
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
        ItemObj itemObjScr= AddItemObjScript(item);

        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.MainItemsList.Count);
        itemObjScr.ItemId = itemList.MainItemsList[ranId].ItemId;
        itemObjScr.MainItemsInfo = itemList.MainItemsList[ranId];
        itemList.MainItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    void GenerateRareItem(GameObject item, EItemType type, ItemsData itemList)
    {
        ItemObj itemObjScr = AddItemObjScript(item);
        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.RareItemsList.Count);
        itemObjScr.ItemId = itemList.RareItemsList[ranId].ItemId;
        itemObjScr.RareItemsInfo = itemList.RareItemsList[ranId];
        itemList.RareItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    void GenerateJunkItem(GameObject item, EItemType type,ItemsData itemList)
    {
        //Debug.Log(itemList.NumberJunkItems);
        ItemObj itemObjScr = AddItemObjScript(item);
        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.JunkItemsList.Count);
        itemObjScr.ItemId = itemList.JunkItemsList[ranId].ItemId;
        itemObjScr.JunkItemInfo = itemList.JunkItemsList[ranId];
        itemList.JunkItemsList.RemoveAt(ranId);
        //itemList.NumberJunkItems--;
        convertListToSt(itemList);
    }

    void Update()
    {

    }
    void convertListToSt(ItemsData itemList)
    {
        mainlist = "";
        rarelist = "";
        junkList = "";
        for (int i = 0; i < itemList.MainItemsList.Count; i++)
            mainlist += itemList.MainItemsList[i].NameMainItem + ",";

        for (int i = 0; i < itemList.RareItemsList.Count; i++)
            rarelist += itemList.RareItemsList[i].NameRareItem + ",";

        for (int i = 0; i < itemList.JunkItemsList.Count; i++)
            junkList += itemList.JunkItemsList[i].NameJunkItem + ",";

        
    }
    ItemObj AddItemObjScript(GameObject item)
    {
        ItemObj itemObjScr;
        if (item.GetComponent<ItemObj>() == null)
            itemObjScr = item.AddComponent<ItemObj>();
        else
        {
            itemObjScr = item.GetComponent<ItemObj>();
            itemObjScr.MainItemsInfo = new MainItemsData();
            itemObjScr.RareItemsInfo = new RareItemsData();
            itemObjScr.JunkItemInfo = new JunkItemsData();
        }

        return itemObjScr;
    }
    void OnGUI()
    {
        GUI.color = Color.red;
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = Color.red;
        myStyle.fontSize = 100;
        GUI.Label(new Rect(10, 0, 400, 100), mainlist + " ", myStyle);
        GUI.Label(new Rect(10, 100, 400, 100), rarelist + " ", myStyle);
        GUI.Label(new Rect(10, 200, 400, 100), junkList + " ", myStyle);

    }
}
