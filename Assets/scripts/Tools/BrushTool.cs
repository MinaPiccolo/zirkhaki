using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : Tools
{
    private int count = 0;
    private void OnEnable()
    {
        ToolController.ChangePositionEvent += ChangePosition;
        ObstacleObj.ObstacleHitEvent += ObstacleHited;
    }
    void ObstacleHited(ObstacleData ObstacleType, GameObject go)
    {
        if (ToolController.Instance.selectTool == EToolsType.brush)
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
    void OnMouseDown()
    {
        GetComponent<Collider2D>().enabled = false;
        ToolHit(ToolType,gameObject);
    }
 
    public void ChangePosition(Vector2 pos)
    {
        if (ToolController.Instance.selectTool == ToolType)
            transform.position = pos;
        else
            transform.position = new Vector3(0, -10, 0);
    }
}
