using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelTool : Tools
{
    private void OnEnable()
    {
        ToolController.ChangePositionEvent += ChangePosition;
        ObstacleObj.ObstacleHitEvent += ObstacleHited;
    }
    void ObstacleHited(GameObject go)
    {
        ObstacleObj obsScr = go.GetComponent<ObstacleObj>();
        if (ToolController.Instance.selectTool == EToolsType.shovel)
            if (obsScr.ObstacleInfoObj.UseTool == ToolType)
            {
                obsScr.CountClick++;
                Debug.Log("Hit");
                if (obsScr.CountClick >= obsScr.ObstacleInfoObj.NumberScrachToDestroy)
                {
                    StuffDestroyObstacle(go);
                }
            }
    }
    void OnMouseDown()
    {
        GetComponent<Collider2D>().enabled = false;
        ToolHit(ToolType, gameObject);
    }
    public void ChangePosition(Vector2 pos)
    {
        if (ToolController.Instance.selectTool == ToolType)
            transform.position = pos;
        else
            transform.position = new Vector3(0, -10, 0);
    }
}
