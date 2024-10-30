using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObjectEventTrigger : MonoBehaviour
{
    [SerializeField] private GameObject QuestPanel;

    void OnMouseUp()
    {
        QuestPanel.SetActive(true);
        gameObject.SetActive(false);
        Debug.Log("mouse released on object(collider)");
    }
   
}
