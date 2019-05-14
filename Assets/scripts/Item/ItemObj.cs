using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    public int ItemId;
    public int IndexTypeItem;
    public EItemType ItemType;
   // public ItemsData itemList;
    public MainItemsData MainItemsInfo;
    public RareItemsData RareItemsInfo;
    public JunkItemsData JunkItemInfo;

    void OnMouseDown()
    {
        OnClickItem();
    }
    void OnClickItem()
    {
        if(ItemType==EItemType.Main)
        {
            //ItemController.Instance.MainPieseseAdd(itemList,IndexTypeItem);
            //MainItemsInfo.NumberOfFound++;
            InventoryController.Instance.AddMainItem(MainItemsInfo);// itemList.MainItemsList[IndexTypeItem]);
        }
        else if (ItemType == EItemType.Rare)
        {
            //ItemController.Instance.RarePieseseAdd(itemList,IndexTypeItem);
            //RareItemsInfo.NumberOfFound++;
            InventoryController.Instance.AddRareItem(RareItemsInfo);//itemList.RareItemsList[IndexTypeItem]);//RareItemsInfo);
        }
        else if (ItemType == EItemType.Junk)
        {
            InventoryController.Instance.AddJunkItem(JunkItemInfo);// itemList.JunkItemsList[IndexTypeItem]);//JunkItemInfo);
        }
        SimplePool.Despawn(gameObject);
    }
  
}
