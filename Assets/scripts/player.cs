using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public int[] inventory;
    static player _instance;
    public static player Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }
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
    }
    int checkInventoryFull()
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
