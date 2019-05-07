using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wholeMap : MonoBehaviour
{
    public RectTransform parentMaps, pivotGO,wholeMain;
    public Transform parentMap;
    public int currentState;
    public GameObject[] maps;
    private bool isZoomin,isZoomOut,isMoving;
    public float scaleLimit,speed,speedMove;
    private Vector3 scaleVec, destinationPos;

    public static wholeMap Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        scaleVec = new Vector3(scaleLimit, scaleLimit, scaleLimit);
    }
   
    public void zoomin()
    {

        //Transform targetTransform = maps[currentState].transform;
        //Vector3 posTarget = maps[currentState].transform.position;
        //targetTransform.SetParent(parentMaps.parent, false);
        //parentMaps.position += posTarget - targetTransform.position;


        // ScaleAround(parentMap.transform, maps[currentState].transform, scaleVec);
        //Vector2 pivotResult = maps[currentState].transform.position;
        //pivotResult.x += maps[currentState].transform.position.x- parentMaps.transform.position.x;
        //pivotResult.y += maps[currentState].transform.position.y - parentMaps.transform.position.y;

        //pivotResult.x += maps[currentState].transform.position.x - parentMaps.transform.position.x;
        //pivotResult.y +
        Vector3 pos;
        //SetPivot(maps[currentState].GetComponent<RectTransform>(), piv, out pivot, out pos);
        //pivotResult.x -= parentMaps.pivot.x;
        //pivotResult.y -= parentMaps.pivot.y;
        getResultPos(maps[currentState].GetComponent<RectTransform>(), maps[currentState].transform.position, out pos);
        Debug.Log(pos);


        //Vector2 pivot = maps[currentState].transform.position;
        //pivot.x += maps[currentState].transform.position.x;
        //pivot.x = Mathf.Abs(pivot.x);
        //SetPivot(parentMaps, pivot);
        destinationPos=   parentMaps.localPosition- pos;
        isMoving = true;

        //parentMaps.localPosition -= pos;
        // pivotGO.position = maps[currentState].transform.position;
        // parentMaps.SetParent(pivotGO);

        //pivotGO.localPosition = maps[currentState].GetComponent<RectTransform>().localPosition;
        // parentMaps.SetParent(pivotGO);

        //  parentMaps.pivot = maps[currentState].transform.position;
        //  parentMaps.position = targetPivot.position - parentMaps.pivot;
        //Debug.Log(targetPivot.position);
        //Debug.Log(parentMaps.pivot);
        //Vector3 pos = parentMaps.position;
        //pos.x = targetPivot.position.x + parentMaps.pivot.x;
        //pos.y = targetPivot.position.y + parentMaps.pivot.y;
        //parentMaps.position = pos;
        //Vector3 posMap = parentMaps.position;
        //Debug.Log(targetPivot.position);
        //Debug.Log(parentMaps.GetComponent<RectTransform>().position);
        //Debug.Log(parentMaps.transform.position);

        // parentMaps.transform.position = targetPivot.position;
        // Debug.Log(parentMaps.pivot);
        //parentMaps.position = Vector3.zero;
        //parentMaps.position += targetPivot.position - parentMaps.position;
        //parentMaps.GetComponent<RectTransform>().position = Vector3.zero;
        // isZoomin = true;
    }
    //public  void SetPivot(RectTransform rectTransform, Vector2 pivot)
    // {

    //     Vector2 size = rectTransform.rect.size;
    //     Vector2 deltaPivot = rectTransform.pivot - pivot;
    //     Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
    //     rectTransform.pivot = pivot;
    //     rectTransform.localPosition -= deltaPosition;
    // }
    public void getResultPos(RectTransform rectTransform, Vector2 pivot,out Vector3 deltaPositionRes)
    {
        //if (rectTransform == null) return;

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = /*rectTransform.pivot - */pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        deltaPositionRes =deltaPosition;
        //rectTransform.pivot = pivot;
        //rectTransform.localPosition -= deltaPosition;
    }
    //public void ScaleAround(Transform target, Transform pivot, Vector3 scale)
    //{
    //    Transform pivotParent = pivot.parent;
    //    Vector3 pivotPos = pivot.position;
    //    pivot.parent = target;
    //    target.localScale = scale;
    //    target.position += pivotPos - pivot.position;
    //    pivot.parent = pivotParent;
    //}
    void Update()
    {
        if (isZoomin)
            zoominScale(pivotGO);
        if (isZoomOut)
            zoomoutScale(pivotGO);

        if(isMoving)
            movingPointToPoint(parentMaps, destinationPos);

    }

    public void zoominScale(RectTransform rec)
    {
        Vector3 sc = rec.GetComponent<RectTransform>().localScale;
        sc.x += speed * Time.deltaTime;
        sc.y += speed * Time.deltaTime;
        rec.GetComponent<RectTransform>().localScale = sc;

        if (sc.x >= scaleLimit)
        {
            rec.GetComponent<RectTransform>().localScale = new Vector3(scaleLimit, scaleLimit);
            isZoomin = false;
            parentMaps.transform.SetParent(wholeMain, true);

        }
    }
    void zoomoutScale(RectTransform rec)
    {
        Vector3 sc = rec.GetComponent<RectTransform>().localScale;
        sc.x -= speed * Time.deltaTime;
        sc.y -= speed * Time.deltaTime;
        rec.GetComponent<RectTransform>().localScale = sc;

        if (sc.x <= 1)
        {
           // parentMaps.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
            isZoomOut = false;
        }
    }


    void movingPointToPoint(RectTransform startRec,Vector3 destinationPos)
    {

        startRec.localPosition = Vector2.MoveTowards(startRec.localPosition, destinationPos, speedMove * Time.deltaTime);

        if (Vector3.Distance(startRec.localPosition, destinationPos) < 0.05f)
        {
            isMoving = false;
            startRec.localPosition = destinationPos;
            pivotGO.position = maps[currentState].transform.position;
            parentMaps.SetParent(pivotGO);
            isZoomin = true;
        }
    }
}
