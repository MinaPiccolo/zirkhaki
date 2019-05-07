using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapMoving : MonoBehaviour
{
    public RectTransform parentMaps, pivotGO, wholeMain;
    public int currentState;
    public GameObject[] states;
    private bool isZoomin, isZoomOut, isMoving;
    public float scaleLimit, speed, speedMove;
    private Vector3 destinationPos;

    public static mapMoving Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        //scaleVec = new Vector3(scaleLimit, scaleLimit, scaleLimit);
    }

    public void zoomin()
    {
        Vector3 pos;
        getResultPos(states[currentState].GetComponent<RectTransform>(), states[currentState].transform.position, out pos);
        destinationPos = parentMaps.localPosition - pos;
        isMoving = true;
    }
    void movingPointToPoint(RectTransform startRec, Vector3 destinationPos)
    {

        startRec.localPosition = Vector2.MoveTowards(startRec.localPosition, destinationPos, speedMove * Time.deltaTime);

        if (Vector3.Distance(startRec.localPosition, destinationPos) < 0.05f)
        {
            isMoving = false;
            startRec.localPosition = destinationPos;
            pivotGO.position = states[currentState].transform.position;
            parentMaps.SetParent(pivotGO);
            isZoomin = true;
        }
    }

    public void getResultPos(RectTransform rectTransform, Vector2 pivot, out Vector3 deltaPositionRes)
    {

        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = /*rectTransform.pivot - */pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y);
        deltaPositionRes = deltaPosition;
    }
  
    void Update()
    {
        if (isZoomin)
            zoominScale(pivotGO);
        if (isZoomOut)
            zoomoutScale(pivotGO);

        if (isMoving)
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


   
}
