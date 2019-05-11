using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelTool : Tools
{
    //private bool isMouseDwn = false, isMouseDrag = false;
    private void OnEnable()
    {
        ToolController.ChangePositionEvent += ChangePosition;
    }
    void OnMouseDown()
    {
        BehaviourTool();
      //  isMouseDwn = true;
    }
    //void OnMouseDrag()
    //{
    //    if (ToolController.Instance.selectTool == ToolType)
    //    {
    //        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    //        //pos.Set(pos.x, transform.position.z, pos.y);
    //        transform.position = pos;
    //    }
    //    else
    //        transform.position = new Vector3(0, -10, 0);
    //}
    //void OnMouseDrag()
    //{
    //    isMouseDrag = true;
    //}

    //void OnMouseExit()
    //{
    //}

    public override void BehaviourTool()
    {
        // base.BehaviourTool();
        UseTool();
    }

    public void ChangePosition(Vector2 pos)
    {
        if (ToolController.Instance.selectTool == ToolType)
            transform.position = pos;
    }

   
}
