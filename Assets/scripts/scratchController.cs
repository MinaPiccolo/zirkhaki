using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scratchController : MonoBehaviour
{
    public GameObject parentScratch, scratchObj;
    void Start()
    {
        generateScratch(5, 10);
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public void generateScratch(int numObj, int scratchToDrop)
    {
        for (int i = 0; i < numObj; i++)
        {
            GameObject sc =controller.Instance.generateGameObj(scratchObj, parentScratch.transform);
            //  sc.transform.localPosition = new Vector2((scratchObj.transform.position.x - 3) + i, scratchObj.transform.position.y);
            sc.transform.localPosition = new Vector2((scratchObj.transform.localPosition.x - 100) + i * 100, scratchObj.transform.localPosition.y);
            sc.GetComponent<scratchableObj>().scratchToDrop = scratchToDrop;
        }
    }
   
   
}
