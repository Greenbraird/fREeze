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
        print("�����ϼ̽��ϴ�");
        IsGamestart = false;
        RestartPanel.SetActive(true);
        Destroy(gameObject);
        // �ٽ� ���� and ���� ȭ������ �� �� �ִ� UI SetActive(true);
        
    }
}
