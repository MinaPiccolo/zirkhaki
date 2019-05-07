using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public GameObject[] itemsGo;
    public Transform parentItems;

    public static ItemController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
   
    void Start()
    {

    }

    public void generateItem(int itemObjIndex,Vector2 pos)
    {
        GameObject item = Controller.Instance.generateGameObj(itemsGo[itemObjIndex], parentItems);
        item.transform.position = new Vector2(pos.x, pos.y + 1);
        item.GetComponent<ItemObj>().index = itemObjIndex;
        item.GetComponent<ItemObj>().type = itemObjIndex + 1;
    }
   
    void Update()
    {
        
    }
}
