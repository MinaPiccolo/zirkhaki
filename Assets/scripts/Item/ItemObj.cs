﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{

    public ItemsData ItemInfoObj;

    //public int ItemId;
    //public EItemType ItemType;
    //public string ItemName;
    //public int PiecesCount;
    //public int FoundPieces;
    //public int index;
    //public int type;
    //void Start()
    //{
    //}
    //void Update()
    //{

    //}

    void OnMouseDown()
    {
        OnClickItem();
    }
    void OnClickItem()
    {
        //int findEmptySlot = Player.Instance.FindEpmtySlot();
        //if (findEmptySlot == -1)
        //    return;
        //Player.Instance.PutItemToInventory(type, findEmptySlot);

        //InventoryController.Instance.AddItemToInventory(ItemInfoObj);//ItemId, ItemType, ItemName, PiecesCount, FoundPieces);
        SimplePool.Despawn(gameObject);
    }
  
}
