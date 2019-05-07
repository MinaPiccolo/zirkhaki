using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public Button startGame;
    public GameObject mapPnl;
    void Start()
    {
        startGame.onClick.AddListener(() => { onClickStartGame(); });
    }

    void onClickStartGame()
    {
        mapPnl.SetActive(true);
    }
    void Update()
    {
        
    }
}
