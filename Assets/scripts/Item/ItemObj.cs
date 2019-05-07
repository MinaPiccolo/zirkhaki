using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemObj : MonoBehaviour
{
    public int index;
    public int type;
    void Start()
    {
  //      GetComponent<Button>().onClick.AddListener(() => { onClickGameObj(); });
    }

    public void collectItems()
    {
    }
    void onClickGameObj()
    {
        if (Player.Instance.checkInventoryFull() != -1)
        {
            Player.Instance.putToInventory(type);
            SimplePool.Despawn(gameObject);
        }
    }
    void Update()
    {
        
    }
}
