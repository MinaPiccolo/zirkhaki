using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text inv;
   
    public static HUD Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void OnEnable()
    {
     
    }
    void Start()
    {
        
    }
    public void showInventory()
    {
        inv.text = "";
        for (int i=0;i<Player.Instance.Inventory.Count;i++)
        {
            inv.text += Player.Instance.Inventory[i] + "/";
        }
    }
  
    void Update()
    {
        
    }
}
