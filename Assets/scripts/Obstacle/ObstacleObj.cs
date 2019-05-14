using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObstacleObj : MonoBehaviour
{
    private bool isMouseDwn = false, isMouseDrag = false;
   // private int count = 0;
    //private EToolsType ToolTypeClicked=EToolsType.non;
    private GameObject ToolGoClicked;

    public delegate void ActionObstacleHit(GameObject Go);
    public static event ActionObstacleHit ObstacleHitEvent;

    public ObstacleData ObstacleInfoObj;
    public int CountClick;
    
    void OnEnable()
    {
        Addressables.LoadAssets<Sprite>("Obstacle", null).Completed += LoadCompleted;
        Tools.DestroyOstacleEvent += DestroyObs;
       // Tools.ToolHitEvent += ToolHit;
    }
    //void ToolHit(EToolsType ToolType,GameObject ToolGo)
    //{
    //    ToolTypeClicked = ToolType;
    //    ToolGoClicked = ToolGo;
    //}

    void LoadCompleted(AsyncOperationHandle<IList<Sprite>> spAs)
    {
        //Debug.Log(spAs.Result[0].name);
        for (int i = 0; i < spAs.Result.Count; i++)
        {
            // Debug.Log(spAs.Result[i].name);
            if (spAs.Result[i].name == "Stone")
            {
                SetSpritBaseOnType(EObstacleType.stone, spAs.Result[i]);
            }
            else if (spAs.Result[i].name == "Hill")
            {
                SetSpritBaseOnType(EObstacleType.hill, spAs.Result[i]);
            }
        }
    }
    void SetSpritBaseOnType(EObstacleType obctacleType, Sprite sp)
    {
        if (ObstacleInfoObj.TypeObstacle == obctacleType)
            GetComponent<SpriteRenderer>().sprite = sp;
    }

   
    void OnMouseDown()
    {
        isMouseDwn = true;
        if (ObstacleInfoObj.UseTool == EToolsType.shovel)
            CheckHitObstacle();
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

        if (ObstacleInfoObj.UseTool == EToolsType.brush)
        {
            if (isMouseDwn && isMouseDrag)
                CheckHitObstacle();
        }
       

    }

    void CheckHitObstacle()
    {
        ObstacleHitEvent(gameObject);
    }

    void DestroyObs(GameObject Go)
    {
        SimplePool.Despawn(Go);
    }

}
