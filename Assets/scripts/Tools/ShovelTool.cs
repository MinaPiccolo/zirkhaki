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
    }
    void ObstacleHited(ObstacleData ObstacleType,GameObject go)
    {
        if (ToolController.Instance.selectTool == EToolsType.shovel)
            if (ObstacleType.UseTool == ToolType)
            {
                Debug.Log("HitOnstacle");
                count++;
                if (count >= ObstacleType.NumberScrachToDestroy)
                {
                    StuffDestroyObstacle(go);
                }
            }
    }
    //void StuffDestroyObstacle(GameObject go)
    //{
    //    Debug.Log("Obstacle Destroy");
    //    ToolController.Instance.PutTool();
    //    ItemController.Instance.GenerateItem(LvlController.Instance.TempeItemsListInfo);
    //    DestroyObstacle(go);
    //}
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
