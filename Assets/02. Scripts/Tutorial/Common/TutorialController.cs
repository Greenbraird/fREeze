using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField]
    private List<TutorialBase> tutorials;

    private TutorialBase currentTutorial = null;
    private int currentIndex = -1;

    void Start()
    {
        SetNextTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTutorial != null)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial(int skipnumber =1)
    {
        //���� Ʃ�丮���� Exit() �޼ҵ� ȣ��
        if(currentTutorial != null)
        {
            currentTutorial.Exit();
        }


        // ������ Ʃ�丮���� �����ߴٸ� CompletedAllTutorials() �޼ҵ� ȣ��
        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        // ���� Ʃ�丮�� ������ currentTutorial�� ���
        currentIndex += skipnumber;
        currentTutorial = tutorials[currentIndex];

        // ���� �ٲ� Ʃ�丮���� Enter() �޼ҵ� ȣ��
        currentTutorial.Enter();
    }

    public void SetSkipNextTutorial()
    {
        //���� Ʃ�丮���� Exit() �޼ҵ� ȣ��
        if (currentTutorial != null)
        {
            currentTutorial.Exit();
        }

        // ������ Ʃ�丮���� �����ߴٸ� CompletedAllTutorials() �޼ҵ� ȣ��
        if (currentIndex >= tutorials.Count - 1)
        {
            CompletedAllTutorials();
            return;
        }

        currentIndex += 2;
        currentTutorial = tutorials[currentIndex];

        currentTutorial.Enter();
    }

    public void CompletedAllTutorials()
    {
        currentTutorial = null;

        PlayerPrefs.SetInt("TutorialEnd", 1);

        // �ൿ ����� ���� ������ �Ǿ��� �� �ڵ� �߰� �ۼ�
        // ����� �� ��ȯ

        Debug.Log("Complete All");
    }
}
