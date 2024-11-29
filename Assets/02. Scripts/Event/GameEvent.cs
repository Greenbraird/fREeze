using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public GameObject RestartPanel;
    public GameObject FinishPanel;

    public void FinishEvent()
    {
        FinishPanel.SetActive(true);
    }

    public void ResetEvent()
    {
        RestartPanel.SetActive(true);
    }
}
