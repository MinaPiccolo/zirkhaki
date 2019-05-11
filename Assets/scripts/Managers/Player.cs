using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<InventoryItem> Inventory;

   
    public static Player Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        Inventory = new List<InventoryItem>();
    }
    //public void PutItemToInventory(int indexItem, int findEmptySlot)
    //{
    //    Inventory[findEmptySlot] = indexItem;
    //    HUD.Instance.showInventory();
    //}
     void Update()
    {
        //if ((Input.touchCount > 0))
        //{
        //    Touch touch = Input.GetTouch(0);
        //    if (touch.phase == TouchPhase.Moved)
        //    {
        //    }
        //}
        //else if ((Input.GetButtonDown(0)))
        //{
        //}
    }
    public void AddItemToInventory(ItemsData itemInfo)// int itemId, EItemType itemType,string itemName,int piecesCount,int foundPieces)
    {


        //InventoryItem item;
       // item.ItemId = itemId;
      //  item.ItemType = itemType;
     //   item.ItemName = itemName;
     //   item.PiecesCount = piecesCount;
      //  item.FoundPieces = foundPieces;
     //   Inventory.Add(item);
    }
//public int FindEpmtySlot()
//    {
//        for (int i = 0; i < Inventory.Count; i++)
//            if (Inventory[i] == 0)
//                return i;

//        return -1;
//    }
  
}
