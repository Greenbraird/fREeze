using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : Singleton<GameSystem>
{
    public GameObject RestartPanel;

    public bool IsGamestart = false;

    public bool touchable = false;

    private void Start()
    {
        RestartPanel = GameObject.Find("Restart Panel");
    }

    public void GameStart()
    {

    }

    public void GameEnd()
    {
        print("실패하셨습니다");
        IsGamestart = false;
        RestartPanel.SetActive(true);
        Destroy(gameObject);
        // 다시 시작 and 메인 화면으로 갈 수 있는 UI SetActive(true);
        
    }
}
