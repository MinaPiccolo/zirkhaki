using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : Tools
{
    private void OnEnable()
    {
        ToolController.ChangePositionEvent += ChangePosition;
        ObstacleObj.ObstacleHitEvent += ObstacleHited;
    }
    void ObstacleHited(GameObject go)
    {
        ObstacleObj obsScr = go.GetComponent<ObstacleObj>();
        if (ToolController.Instance.selectTool == EToolsType.brush)
            if (obsScr.ObstacleInfoObj.UseTool == ToolType)
            {
                Debug.Log("HitOnstacle");
                obsScr.CountClick++;
                if (obsScr.CountClick >= obsScr.ObstacleInfoObj.NumberScrachToDestroy)
                {
                    StuffDestroyObstacle(go);
                }
            }
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
