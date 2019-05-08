using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    public Button LvlBtn;
    //public GameObject mapPnl;
    void Start()
    {
      //  startGame.onClick.AddListener(() => { onClickStartGame(); });
        LvlBtn.onClick.AddListener(() => { OnClickLvlBtn(); });
    }

    //void onClickStartGame()
    //{
    //    mapPnl.SetActive(true);
    //}
    void OnClickLvlBtn()
    {
        SceneManager.LoadScene("gameScene");
    }
    void Update()
    {
        
    }
}
