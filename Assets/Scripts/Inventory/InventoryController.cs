using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Text MainInventoryText,RareInventoryText,JunkInventoryText;
    public int JunkInventoryRemoveAt;
    public List<InventoryItem> MainInventory,RareInventory,JunkInventory;
    string invMain, invRar, invJunk;
    public static InventoryController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        MainInventory = new List<InventoryItem>();
        RareInventory = new List<InventoryItem>();
        JunkInventory = new List<InventoryItem>();
        ShowMainInventory();
        ShowRareInventory();
        ShowJunkInventory();
        convertListToSt();
    }

    #region AddItems
    public void AddMainItem(MainItemsData mainItemsInfo)
    {

        int findIdItemIndex = FindIdItemIndex(mainItemsInfo.ItemId, MainInventory);
        if (findIdItemIndex != -1)
        {
            PieseseAddExist(ref MainInventory, findIdItemIndex);
        }
        else
        {
            InventoryItem item = new InventoryItem();
            item.ItemId = mainItemsInfo.ItemId;
            item.ItemType = EItemType.Main;
            item.ItemName = mainItemsInfo.NameMainItem;
            item.PiecesCount = mainItemsInfo.NumberOfPices;
            item.FoundPieces++;
            item.IsCompleted = CheckPiecesComplete(item.PiecesCount, item.FoundPieces);
            MainInventory.Add(item);
            if (item.IsCompleted)
                DoStuffCompeletedPieces();
        }
        convertListToSt();
        ShowMainInventory();
    }
    public void AddRareItem(RareItemsData rareItemInfo)
    {
        int findIdItemIndex = FindIdItemIndex(rareItemInfo.ItemId, RareInventory);
        if (findIdItemIndex != -1)
        {
            PieseseAddExist(ref RareInventory, findIdItemIndex);
        }
        else
        {
            InventoryItem item = new InventoryItem(); ;
            item.ItemId = rareItemInfo.ItemId;
            item.ItemType = EItemType.Rare;
            item.ItemName = rareItemInfo.NameRareItem;
            item.PiecesCount = rareItemInfo.NumberOfPices;
            item.FoundPieces++;
            item.IsCompleted = CheckPiecesComplete(item.PiecesCount, item.FoundPieces);
            RareInventory.Add(item);
            if (item.IsCompleted)
                DoStuffCompeletedPieces();
        }

        convertListToSt();
        ShowRareInventory();
    }

    public void AddJunkItem(JunkItemsData junkItemInfo)
    {
        if (CheckJunkInventoryIsFull())
            RemoveJunkItem();

        InventoryItem item = new InventoryItem(); ;
        item.ItemId = junkItemInfo.ItemId;
        item.ItemType = EItemType.Junk;
        item.ItemName =junkItemInfo.NameJunkItem;
        item.PiecesCount = 0;
        item.FoundPieces = 0;
        item.IsCompleted = true;
        JunkInventory.Add(item);

        convertListToSt();
        ShowJunkInventory();

    }
    #endregion

    #region RemoveItems
    public void RemoveMainItem(int itemId)
    {
        RemoveItemWithItemId(itemId, MainInventory);
    }
    public void RemoveRareItem(int itemId)
    {
        RemoveItemWithItemId(itemId, RareInventory);
    }
    public void RemoveJunkItem()
    {
        //do property work ,,, sale all them
        for (int i = 0; i < JunkInventoryRemoveAt; i++)
            JunkInventory.RemoveAt(i);
    }
    void RemoveItemWithItemId(int itemId,List<InventoryItem> inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
            if (inventory[i].ItemId == itemId)
                inventory.RemoveAt(i);
    }
    #endregion

    #region logic Functions
    public bool CheckPiecesComplete(int piecesCount, int foundPieces)
    {
        if (foundPieces >= piecesCount)
            return true;
        else
            return false;
    }
    public bool CheckJunkInventoryIsFull()
    {
        if (JunkInventory.Count >= JunkInventoryRemoveAt)
            return true;
        return false;
    }
    void DoStuffCompeletedPieces()
    {

    }

    int FindIdItemIndex(int IdItem, List<InventoryItem> inventory)
    {
        for (int i = 0; i < inventory.Count; i++)
            if (inventory[i].ItemId == IdItem)
                return i;
        return -1;

    }
    public void PieseseAddExist(ref List<InventoryItem> inventory, int index)
    {
        var inv = inventory[index];
        inv.FoundPieces++;
        inv.IsCompleted = CheckPiecesComplete(inv.PiecesCount, inv.FoundPieces);
        inventory[index] = inv;
        if (inventory[index].IsCompleted)
            DoStuffCompeletedPieces();
    }
   
    #endregion



    #region UI
    void ShowMainInventory()
    {
       // ShowInventory(MainInventory, MainInventoryText);
    }
    void ShowRareInventory()
    {
      //  ShowInventory(RareInventory,RareInventoryText);
    }
    void ShowJunkInventory()
    {
       // ShowInventory(JunkInventory,JunkInventoryText);
    }
    void ShowInventory(List<InventoryItem> inventory,Text inventoryText)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryText.text = inventory[i].FoundPieces + "/"+inventory[i].PiecesCount+",";
        }
    }
    #endregion
    void convertListToSt()
    {
        invMain = "";
        invRar = "";
        invJunk = "";
        for (int i = 0; i < MainInventory.Count; i++)
            invMain += MainInventory[i].ItemId + " " + MainInventory[i].FoundPieces + "/" + MainInventory[i].PiecesCount + ",";

        for (int i = 0; i < RareInventory.Count; i++)
            invRar += RareInventory[i].ItemId + " " + RareInventory[i].FoundPieces + "/" + RareInventory[i].PiecesCount + ",";

        for (int i = 0; i < JunkInventory.Count; i++)
            invJunk += JunkInventory[i].ItemId + ",";


    }
    void OnGUI()
    {
        GUI.color = Color.red;
        GUIStyle myStyle = new GUIStyle();
        myStyle.normal.textColor = Color.red;
        myStyle.fontSize = 100;
        GUI.Label(new Rect(10, 300, 400, 100), invMain + " ", myStyle);
        GUI.Label(new Rect(10, 400, 400, 100), invRar + " ", myStyle);
        GUI.Label(new Rect(10, 500, 400, 100), invJunk + "  ", myStyle);

    }
}
