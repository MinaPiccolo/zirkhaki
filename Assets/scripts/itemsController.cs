using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemsController : MonoBehaviour
{
    public GameObject[] itemsGo;
    public Transform parentItems;
    static itemsController _instance;
    public static itemsController Instance
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

    public void dropItems(/*int numItemDrop,*/int itemObjIndex)
    {
        //for (int i = 0; i < numItemDrop; i++)
        //{
            GameObject sc = controller.Instance.generateGameObj(itemsGo[itemObjIndex], parentItems);
            sc.transform.localPosition = new Vector2(itemsGo[0].transform.localPosition.x  , itemsGo[0].transform.localPosition.y+100);
            sc.GetComponent<itemObj>().index = itemObjIndex;
            sc.GetComponent<itemObj>().type = itemObjIndex+1;
        //}
    }
   
    void Update()
    {
        
    }
}
