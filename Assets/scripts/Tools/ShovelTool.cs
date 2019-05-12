using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelTool : Tools
{
    private int count = 0;
    private void OnEnable()
    {
        ToolController.ChangePositionEvent += ChangePosition;
        ObstacleObj.ObstacleHitEvent += ObstacleHited;
        //ObstacleObj.ObstacleHitEndEvent += ObstacleHitEnd;
    }
    void ObstacleHited(ObstacleData ObstacleType,GameObject go)
    {
        if (ToolController.Instance.selectTool == EToolsType.shovel)
            if (ObstacleType.UseTool == ToolType)
            {
                Debug.Log("HitClick");

                count++;
                if (count >= ObstacleType.NumberScrachToDestroy)
                {
                    Debug.Log("Finally Obstacle Hited");
                    DestroyObstacle(go);
                }
            }


        //GetComponent<Collider2D>().enabled = true;
        // Debug.Log(ObstacleType);
    }
    //void ObstacleHitEnd()
    //{
    //    GetComponent<Collider2D>().enabled = true;
    //}
    void OnMouseDown()
    {

       

        GetComponent<Collider2D>().enabled = false;
        ToolHit(ToolType, gameObject);
    }
   
    public void ChangePosition(Vector2 pos)
    {

        //RaycastHit hit;
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ////  Debug.Log(ray.direction);
        //Physics.Raycast(ray, out hit, 1000);
        //// Debug.Log(hit.collider);
        //// if (Physics.Raycast(ray, out hit))
        //{
        //    Debug.Log(hit.transform.name);
        //}


        if (ToolController.Instance.selectTool == ToolType)
            transform.position = pos;
        else
            transform.position = new Vector3(0, -10, 0);
    }

   
}
