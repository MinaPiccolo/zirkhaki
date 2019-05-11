using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public EToolsType ToolType;
    public delegate void ActionUseTool(EToolsType ToolType);
    public static event ActionUseTool UseToolEvent;
    public virtual void BehaviourTool()
    {

    }

   

    public void UseTool()
    {
        UseToolEvent(ToolType);
    }

}
