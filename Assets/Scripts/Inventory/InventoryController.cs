using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public Text MainInventoryText,RareInventoryText,JunkInventoryText;
    public int JunkInventoryRemoveAt;
    public List<InventoryItem> MainInventory,RareInventory,JunkInventory;

    public static InventoryController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        if (PlayerPrefs.GetInt("createInventoriesOnce") == 0)
        {
            PlayerPrefs.SetInt("createInventoriesOnce", 1);
            MainInventory = new List<InventoryItem>();
            RareInventory = new List<InventoryItem>();
            JunkInventory = new List<InventoryItem>();
        }
    }

    #region AddItems
    public void AddMainItem(ItemsData itemInfo)
    {
        InventoryItem item;
        item.ItemId = 1;
        item.ItemType = EItemType.Main;
        item.ItemName = "D";
        item.PiecesCount = 10;
        item.FoundPieces = 1;// "item"+1;
        item.IsCompleted = CheckPiecesComplete(item.PiecesCount, item.FoundPieces);
        MainInventory.Add(item);
        if (item.IsCompleted)
            DoStuffCompeletedPieces();
    }
    public void AddRareItem(ItemsData itemInfo)
    {
        InventoryItem item;
        item.ItemId = 1;
        item.ItemType = EItemType.Rare;
        item.ItemName = "D";
        item.PiecesCount = 10;
        item.FoundPieces = 1;// "item"+1;
        item.IsCompleted = CheckPiecesComplete(item.PiecesCount, item.FoundPieces);
        MainInventory.Add(item);
        if (item.IsCompleted)
            DoStuffCompeletedPieces();
    }

    public void AddJunkItem(ItemsData itemInfo)
    {
        if (CheckJunkInventoryIsFull())
            RemoveJunkItem();

        InventoryItem item;
        item.ItemId = 1;
        item.ItemType = EItemType.Junk;
        item.ItemName = "D";
        item.PiecesCount = 0;
        item.FoundPieces = 0;
        item.IsCompleted = true;
        MainInventory.Add(item);
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
    #endregion



    #region UI
    void ShowMainInventory()
    {
        ShowInventory(MainInventory, MainInventoryText);
    }
    void ShowRareInventory()
    {
        ShowInventory(RareInventory,RareInventoryText);
    }
    void ShowJunkInventory()
    {
        ShowInventory(JunkInventory,JunkInventoryText);
    }
    void ShowInventory(List<InventoryItem> inventory,Text inventoryText)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryText.text = inventory[i].FoundPieces + "/"+inventory[i].PiecesCount+",";
        }
    }
    #endregion
}
