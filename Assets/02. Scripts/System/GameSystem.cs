using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : Singleton<GameSystem>
{
    [SerializeField]
    private GameObject RestartPanel;

    [HideInInspector]
    public bool IsGamestart = false;

    [HideInInspector]
    public bool touchable = false;
    public void GameStart()
    {

    }

    public void GameEnd()
    {
        print("실패하셨습니다");
        IsGamestart = false;
        Destroy(GameSystem.Instance);
        // 다시 시작 and 메인 화면으로 갈 수 있는 UI SetActive(true);
        RestartPanel.SetActive(true);
    }
}
