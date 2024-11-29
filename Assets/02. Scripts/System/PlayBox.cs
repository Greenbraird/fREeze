using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBox : MonoBehaviour
{
    private void OnMouseDown()
    {
        LoadingSceneController.LoadSceneMode("3. Game");
    }
}
