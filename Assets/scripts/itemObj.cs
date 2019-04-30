using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemObj : MonoBehaviour
{
    public int index;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => { onClickGameObj(); });
    }

    public void collectItems()
    {
    }
    void onClickGameObj()
    {
        player.Instance.putToInventory(index);
    }
    void Update()
    {
        
    }
}
