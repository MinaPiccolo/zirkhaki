using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolController : MonoBehaviour
{
    public Button[] ToolsBtn;
    public string[] ToolsName;
    public EToolsType selectTool= EToolsType.non;
    private bool isMoveTool;

    public delegate void ActionChangePositon(Vector2 pos);
    public static event ActionChangePositon ChangePositionEvent;

    public static ToolController Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < ToolsBtn.Length; i++)
            ClickBtns(ToolsBtn[i], i);
    }
    void ClickBtns(Button btn, int index)
    {
        btn.onClick.AddListener(() => { OnclickToolBtn(index); });
    }
    public void InitialHUDLvl()
    {
        for (int i = 0; i < ToolsBtn.Length; i++)
        {
            ToolsBtn[i].GetComponentInChildren<Text>().text = ToolsName[i];
        }
    }
    void OnclickToolBtn(int index)
    {
        selectTool = ConvertInexToEnumType(index);
        for (int i = 0; i < ToolsBtn.Length; i++)
            ToolsBtn[i].GetComponent<Image>().color = Color.white;
        ToolsBtn[index].GetComponent<Image>().color = Color.blue;

        TakeTool();
    }
    void TakeTool()
    {
        isMoveTool = true;
    }
    void Update()
    {
        if (isMoveTool)
            ChangePositionEvent(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
    EToolsType ConvertInexToEnumType(int index)
    {
        switch (index)
        {
            case 0:
                return EToolsType.shovel;
            case 1:
                return EToolsType.brush;

            default:
                return EToolsType.non;
        }

    }





    //public void GenerateToolsInHUD()
    //{

    //}
    //void Update()
    //{
        
    //}
}
