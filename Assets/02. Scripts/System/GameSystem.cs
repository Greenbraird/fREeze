using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : Singleton<GameSystem>
{
    [SerializeField]
    private GameObject RestartPanel;

    [HideInInspector]
     public bool IsGamestart;

    [HideInInspector]
    public bool touchable = true;
    public void GameStart()
    {

    }

    public void GameEnd()
    {
        print("�����ϼ̽��ϴ�");
        // �ٽ� ���� and ���� ȭ������ �� �� �ִ� UI SetActive(true);
        RestartPanel.SetActive(true);
    }
}
