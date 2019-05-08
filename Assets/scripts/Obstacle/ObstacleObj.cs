using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObj : MonoBehaviour
{
  //  public int scratchToDrop;
    private bool isMouseDwn = false, isMouseDrag = false;
    private int count = 0;

    private Ray ray;
    private RaycastHit hit;

    public ObstacleData ObstacleInfoObj;


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
        ScratchObstacle();
    }

    void ScratchObstacle()
    {
        //Debug.Log(isMouseDrag);
        if (isMouseDwn && isMouseDrag)
            count++;
        if (count >= ObstacleInfoObj.NumberScrachToDestroy)
        {
            ItemController.Instance.GenerateItem(0,new Vector2(transform.position.x, transform.position.y+0.2f));
            SimplePool.Despawn(gameObject);
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
