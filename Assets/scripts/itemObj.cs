using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemObj : MonoBehaviour
{
    public int index;
    public int type;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { onClickGameObj(); });
    }

    public void collectItems()
    {
    }
    void onClickGameObj()
    {
        if (player.Instance.checkInventoryFull() != -1)
        {
            player.Instance.putToInventory(type);
            simplePool.Despawn(gameObject);
        }
    }
    void Update()
    {
        
    }
}
