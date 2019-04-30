using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scratchableObj : MonoBehaviour
{
    public int scratchToDrop;
    private bool isMouseDwn = false, isMouseDrag = false;
    private int count = 0;

    private Ray ray;
    private RaycastHit hit;


    void OnMouseDown()
    {
        isMouseDwn = true;
    }
    void OnMouseDrag()
    {
        isMouseDrag = true;
    }

    void OnMouseExit()
    {
        scratchCount();
    }

    void scratchCount()
    {
        if (isMouseDwn && isMouseDrag)
            count++;
        if (count >= scratchToDrop)
        {
            simplePool.Despawn(gameObject);
            itemsController.Instance.dropItems(5, 0);

        }
    }


    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {

        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //if (Physics.Raycast(ray,out hit))
        //{
        //    Debug.Log(hit.collider.name);
        //}
    }
}
