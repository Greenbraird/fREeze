using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSystem : Singleton<GameSystem>
{
    [SerializeField]
    private GameObject RestartPanel;

    public bool IsGamestart = false;

    public bool touchable = false;
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
