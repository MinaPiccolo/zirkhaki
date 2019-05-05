using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wholeMap : MonoBehaviour
{
    public RectTransform parentMaps;
    public Transform parentMap;
    public int currentState;
    public GameObject[] maps;
    private bool isZoomin,isZoomOut;
    public float scaleLimit,speed;
    private Vector3 scaleVec;
    static wholeMap _instance;
    public static wholeMap Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        scaleVec = new Vector3(scaleLimit, scaleLimit, scaleLimit);
    }
    public void zoomin(int i)
    {
        Transform targetTrans = maps[currentState].transform;
        Transform parentTrans = parentMap.transform;
        Transform pivotParent = targetTrans.parent;
        targetTrans.parent = parentTrans;
        parentTrans.localScale = scaleVec;
        parentTrans.position += targetTrans.position - parentTrans.position;
        targetTrans.parent = pivotParent;
    }
    public void zoomin()
    {
        Transform currentStateTransform = maps[currentState].transform;

        maps[currentState].transform.parent = parentMaps.transform;
        parentMaps.position += currentStateTransform.position - parentMaps.position;
        maps[currentState].transform.parent = parentMaps.transform;
        // parentMaps.pivot = pivotChange.pivot = curentStateRecT.position;
        // parentMaps.GetComponent<RectTransform>().position = Vector3.zero;
        isZoomin = true;
    }
    void Update()
    {
        if (isZoomin)
            zoominScale();
        if (isZoomOut)
            zoomoutScale();
    }

    public void zoominScale()
    {
        Vector3 sc = parentMaps.GetComponent<RectTransform>().localScale;
        sc.x += speed * Time.deltaTime;
        sc.y += speed * Time.deltaTime;
        parentMaps.GetComponent<RectTransform>().localScale = sc;

        if (sc.x >= scaleLimit)
        {
            parentMaps.GetComponent<RectTransform>().localScale = new Vector3(scaleLimit, scaleLimit);
            isZoomin = false;
        }
    }
    void zoomoutScale()
    {
        Vector3 sc = parentMaps.GetComponent<RectTransform>().localScale;
        sc.x -= speed * Time.deltaTime;
        sc.y -= speed * Time.deltaTime;
        parentMaps.GetComponent<RectTransform>().localScale = sc;

        if (sc.x <= 1)
        {
            parentMaps.GetComponent<RectTransform>().localScale = new Vector3(1, 1);
            isZoomOut = false;
        }
    }
}
