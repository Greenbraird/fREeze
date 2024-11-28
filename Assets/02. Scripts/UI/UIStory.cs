using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIStory : MonoBehaviour
{
    public GameObject[] storyUis;

    int storyCount = 1;

    public void NextStory()
    {
        if(storyCount == storyUis.Length)
        {
            EndStroy();
            return;
        }

        GameObject currentStroy = storyUis[storyCount];
        Image currentStroyImage = currentStroy.GetComponent<Image>();
        if (storyCount ==0)
        {
            currentStroy.SetActive(true);
            currentStroyImage.DOFade(0, 2).From();
        }
        else
        {
            storyUis[storyCount-1].SetActive(false);
            currentStroy.SetActive(true);
            currentStroyImage.DOFade(0, 2).From();
        }

        storyCount++;
    }

    void EndStroy()
    {
        LoadingSceneController.LoadSceneMode("2. Main");
    }
}
