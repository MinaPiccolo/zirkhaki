using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObstacleObj : MonoBehaviour
{
  //  public int scratchToDrop;
   // private bool isMouseDwn = false, isMouseDrag = false;
    private int count = 0;

    //private Ray ray;
    //private RaycastHit hit;


    public ObstacleData ObstacleInfoObj;

    void OnEnable()
    {
        Tools.UseToolEvent += Hitable;
        Addressables.LoadAsset<Sprite[]>("Stone").Completed += LoadCompleted;
       // Addressables.LoadAsset<Sprite>("Hill").Completed += LoadCompleted;
       
    }
    void LoadCompleted(AsyncOperationHandle<Sprite[]> spAs)
    {
        Debug.Log(spAs.Result.Length);
        for (int i = 0; i < spAs.Result.Length; i++)
        {
            Debug.Log(spAs.Result[i].name);
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
    void SetSpritBaseOnType(EObstacleType obctacleType,Sprite sp)
    {
        if (ObstacleInfoObj.TypeObstacle == obctacleType)
            GetComponent<SpriteRenderer>().sprite = sp;
    }
        //void OnMouseDown()
        //{
        //    isMouseDwn = true;
        //}
        //void OnMouseDrag()
        //{
        //    isMouseDrag = true;
        //}

        //void OnMouseExit()
        //{
        //    ScratchObstacle();
        //}

        //void ScratchObstacle()
        //{
        //    if (ObstacleInfoObj.UseTool == EToolsType.shovel)
        //    {
        //        if (isMouseDwn && isMouseDrag)
        //            count++;
        //        if (count >= ObstacleInfoObj.NumberScrachToDestroy)
        //        {
        //            ItemController.Instance.GenerateItem(transform.position);
        //            SimplePool.Despawn(gameObject);
        //        }
        //    }
        //    else if (ObstacleInfoObj.UseTool == EToolsType.brush)
        //    {
        //        if (isMouseDwn && isMouseDrag)
        //            count++;
        //        if (count >= ObstacleInfoObj.NumberScrachToDestroy)
        //        {
        //            ItemController.Instance.GenerateItem(transform.position);
        //            SimplePool.Despawn(gameObject);
        //        }
        //    }

        //}



        void Hitable(EToolsType ToolType)
    {
        Debug.Log("omad");
        Debug.Log(ToolType);
        if (ObstacleInfoObj.UseTool == ToolType)
        {
            count++;
            if (count >= ObstacleInfoObj.NumberScrachToDestroy)
            {
                ItemController.Instance.GenerateItem(transform.position);
                SimplePool.Despawn(gameObject);
            }

            Debug.Log("hit");
        }
        else
            Debug.Log("wrongTool");
    }

   

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
