using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceDailyQuest : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> questPrefabs = new List<GameObject>();

    [SerializeField]
    private Transform parentTransform;

    void Start()
    {
        InstanceQuest();
    }

    void InstanceQuest()
    {
        while (questPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, questPrefabs.Count);

            Instantiate(questPrefabs[randomIndex], parentTransform);
            questPrefabs.RemoveAt(randomIndex);
        }
    }    
}
