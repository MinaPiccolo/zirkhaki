using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject[] itemsGo;
    public Transform parentItems;
    string mainlist, rarelist, junkList;
    public static ItemController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        convertListToSt(LvlController.Instance.TempItemInfo);
    }
    #region Generate item
   
    public void GenerateItem(ItemsData itemList, Vector2 ObstaclePos)
    {
        convertListToSt(itemList);
      

        EItemLevelSate state;
        StatePiecesItemsLevel(itemList,out state);
        if (state== EItemLevelSate.oneMainItemRemaining)
        {
            GameObject item = GenerateGameObject(itemList, ObstaclePos);
            Debug.Log("Last Part of main");
            if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count > 0)
                RandomGenerateItem(item, EItemType.Junk, EItemType.Rare, itemList);
            else if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count == 0)
                RandomGenerateItem(item, EItemType.Junk, itemList);
            else if (itemList.JunkItemsList.Count == 0 && itemList.RareItemsList.Count > 0)
                RandomGenerateItem(item, EItemType.Rare, itemList);
            else
                RandomGenerateItem(item, EItemType.Main, itemList);
        }
        else if (state == EItemLevelSate.hasItems)
        {
            GameObject item = GenerateGameObject(itemList, ObstaclePos);
            if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count > 0)
            {
                RandomGenerateItem(item, EItemType.Junk, EItemType.Rare, EItemType.Main, itemList);
            }
            else if (itemList.JunkItemsList.Count > 0 && itemList.RareItemsList.Count == 0 )
            {
                RandomGenerateItem(item, EItemType.Junk, EItemType.Main, itemList);
            }
            else if (itemList.JunkItemsList.Count == 0 && itemList.RareItemsList.Count > 0)
            {
                RandomGenerateItem(item, EItemType.Rare, EItemType.Main, itemList);
            }
            else if (itemList.JunkItemsList.Count == 0 && itemList.RareItemsList.Count == 0)
            {
                RandomGenerateItem(item,EItemType.Main, itemList);
            }
        }
        else
        {
            Debug.Log("FinishItemLvl");
        }
    }
    GameObject GenerateGameObject(ItemsData itemList, Vector2 ObstaclePos)
    {
        GameObject item = Controller.Instance.generateGameObj(itemList.ItemLocation, parentItems);
        Vector2 pos = ObstaclePos;// itemList.ItemLocation.position;
        pos.x += 1;
        item.transform.position = pos;
        return item;
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

    void StatePiecesItemsLevel(ItemsData itemList, out EItemLevelSate state)
    {

        if (itemList.MainItemsList.Count <= 1)
        {
            if (itemList.MainItemsList.Count == 0)
            {
                state = EItemLevelSate.finishItem;
                return;
            }
            if (itemList.MainItemsList[0].NumberOfFound + 1 >= itemList.MainItemsList[0].NumberOfPices)
            {
                state = EItemLevelSate.oneMainItemRemaining;
                return;
            }
        }
        state = EItemLevelSate.hasItems;
    }
    void GenerateMainItem(GameObject item, EItemType type, ItemsData itemList)
    {
        ItemObj itemObjScr= AddItemObjScript(item);

        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.MainItemsList.Count);
        itemObjScr.ItemId = itemList.MainItemsList[ranId].ItemId;
        itemObjScr.IndexTypeItem = ranId;
        //itemObjScr.itemList = itemList;
        itemObjScr.MainItemsInfo = itemList.MainItemsList[ranId];
        MainPieseseAdd(itemList, ranId);
        if (CheckPiecesComplete(itemList.MainItemsList[ranId].NumberOfPices,itemList.MainItemsList[ranId].NumberOfFound))
            itemList.MainItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    void GenerateRareItem(GameObject item, EItemType type, ItemsData itemList)
    {
        ItemObj itemObjScr = AddItemObjScript(item);
        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.RareItemsList.Count);
        itemObjScr.ItemId = itemList.RareItemsList[ranId].ItemId;
        itemObjScr.IndexTypeItem = ranId;
        //itemObjScr.itemList = itemList;
        itemObjScr.RareItemsInfo = itemList.RareItemsList[ranId];
        RarePieseseAdd(itemList, ranId);
        if (CheckPiecesComplete(itemList.RareItemsList[ranId].NumberOfPices, itemList.RareItemsList[ranId].NumberOfFound))
            itemList.RareItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    void GenerateJunkItem(GameObject item, EItemType type,ItemsData itemList)
    {
        ItemObj itemObjScr = AddItemObjScript(item);
        itemObjScr.ItemType = type;
        int ranId = Random.Range(0, itemList.JunkItemsList.Count);
        itemObjScr.ItemId = itemList.JunkItemsList[ranId].ItemId;
        //itemObjScr.JunkItemInfo = itemList.JunkItemsList[ranId];
        itemList.JunkItemsList.RemoveAt(ranId);
        convertListToSt(itemList);
    }
    #endregion


    public void MainPieseseAdd(ItemsData itemList,int index)
    {
        var a = itemList.MainItemsList[index];
        a.NumberOfFound++;
        itemList.MainItemsList[index] = a;
    }
    public void RarePieseseAdd(ItemsData itemList,int index)
    {
        var a = itemList.RareItemsList[index];
        a.NumberOfFound++;
        itemList.RareItemsList[index] = a;
    }




    public bool CheckPiecesComplete(int piecesCount, int foundPieces)
    {
        if (foundPieces >= piecesCount)
            return true;
        else
            return false;
    }
   
    void convertListToSt(ItemsData itemList)
    {
        mainlist = "";
        rarelist = "";
        junkList = "";
        for (int i = 0; i < itemList.MainItemsList.Count; i++)
            mainlist += itemList.MainItemsList[i].NameMainItem + " " + itemList.MainItemsList[i].NumberOfFound + "/" + itemList.MainItemsList[i].NumberOfPices + ",";

        for (int i = 0; i < itemList.RareItemsList.Count; i++)
            rarelist += itemList.RareItemsList[i].NameRareItem + " " + itemList.RareItemsList[i].NumberOfFound + "/" + itemList.RareItemsList[i] .NumberOfPices+ ",";

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
            // itemObjScr.itemList = new ItemsData();
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
        myStyle.fontSize = 80;
        GUI.Label(new Rect(10, 0, 400, 100), mainlist + " ", myStyle);
        GUI.Label(new Rect(10, 80, 400, 100), rarelist + " ", myStyle);
        GUI.Label(new Rect(10, 160, 400, 100), junkList + " ", myStyle);

    }
}
