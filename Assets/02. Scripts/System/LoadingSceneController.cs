using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image progressBar;

    public static void LoadSceneMode(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("0. Loading");
    }

    void Start()
    {
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.8f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                // ���� Ÿ�̸� ����
                timer += Time.deltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.8f, 1f, timer);

                // ���α׷����ٰ� 1�� �����ϸ� �� Ȱ��ȭ
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
