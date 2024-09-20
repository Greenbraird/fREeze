using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public string SceneName;

    public void StartLoadingScene()
    {
        LoadingSceneController.LoadSceneMode(SceneName);
    }
}
