using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text inv;
    static HUD _instance;
    public static HUD Instance
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
    public void showInventory()
    {
        inv.text = "";
        for (int i=0;i<player.Instance.inventory.Length;i++)
        {
            inv.text += player.Instance.inventory[i] + "/";
        }
    }
    void Update()
    {
        
    }
}
