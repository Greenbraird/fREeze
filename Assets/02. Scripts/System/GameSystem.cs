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
        print("�����ϼ̽��ϴ�");
        IsGamestart = false;
        Destroy(GameSystem.Instance);
        // �ٽ� ���� and ���� ȭ������ �� �� �ִ� UI SetActive(true);
        RestartPanel.SetActive(true);
    }
}
