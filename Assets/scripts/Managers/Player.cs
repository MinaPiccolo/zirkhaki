using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int[] inventory;

    public static Player Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    //public int CheckInventoryFull
    //{
    //    get {return checkInventoryFull(); }
    //}

    void Start()
    {
        
    }
    public void putToInventory(int indexItem)
    {
        int findEmptySlot = checkInventoryFull();
        if (findEmptySlot !=-1)
        {
            inventory[findEmptySlot] = indexItem;

        }
        HUD.Instance.showInventory();
    }
  public int checkInventoryFull()
    {
        for (int i = 0; i < inventory.Length; i++)
            if (inventory[i] == 0)
                return i;

        return -1;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
