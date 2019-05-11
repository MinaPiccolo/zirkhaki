using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : Tools
{
    private bool isMouseDwn = false, isMouseDrag = false;
    private void OnEnable()
    {
        ToolController.ChangePositionEvent += ChangePosition;
    }
    void OnMouseDown()
    {
        isMouseDwn = true;
    }
    void OnMouseDrag()
    {
        isMouseDrag = true;
        //if (ToolController.Instance.selectTool == ToolType)
        //    transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //else
        //    transform.position = new Vector3(0, -10, 0);

    }

    void OnMouseExit()
    {
        BehaviourTool();
    }

    public override void BehaviourTool()
    {
        // base.BehaviourTool();
        if (isMouseDwn && isMouseDrag)
        {
            UseTool();
        }
    }

    public void ChangePosition(Vector2 pos)
    {
        if (ToolController.Instance.selectTool == ToolType)
            transform.position = pos;
    }
}
