using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    public int ItemId;
    public EItemType ItemType;
    public MainItemsData MainItemsInfo;
    public RareItemsData RareItemsInfo;
    public JunkItemsData JunkItemInfo;
   // public int NumberJunk;
    //public ItemsData ItemInfoObj;

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
        if(ItemType==EItemType.Main)
        {
            MainItemsInfo.NumberOfFound++;
            InventoryController.Instance.AddMainItem(MainItemsInfo);
        }
        else if (ItemType == EItemType.Rare)
        {
            RareItemsInfo.NumberOfFound++;
            InventoryController.Instance.AddRareItem(RareItemsInfo);
        }
        else if (ItemType == EItemType.Junk)
        {
            InventoryController.Instance.AddJunkItem(JunkItemInfo);
        }

        SimplePool.Despawn(gameObject);
    }
  
}
