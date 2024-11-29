using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUpdate : MonoBehaviour
{
    public Transform player; // �÷��̾�(�Ǵ� ī�޶�) ��ġ
    public float renderDistance = 150f; // ������ Ȱ��ȭ �Ÿ�
    public GameObject[] prefabParents; // ������ ������Ʈ �迭

    private void Start()
    {
        // ��� ������Ʈ�� ���� ������� ����
        if (prefabParents.Length == 0)
        {
            prefabParents = GameObject.FindGameObjectsWithTag("Map");

        }
        StartCoroutine(CheckDistanceRoutine());
    }

    private void Update()
    {
        
    }

    IEnumerator CheckDistanceRoutine()
    {
        while (true)
        {
            foreach (GameObject prefabParent in prefabParents)
            {
                if (prefabParent == null) continue;

                // �������� �ڽ� ��ȸ
                foreach (Transform child in prefabParent.transform)
                {
                    if (child == null) continue;

                    // �Ÿ� ���
                    float distance = Vector3.Distance(player.position, child.position);

                    // �Ÿ� ���ǿ� ���� Ȱ��ȭ/��Ȱ��ȭ
                    bool shouldActivate = distance <= renderDistance;
                    if (child.gameObject.activeSelf != shouldActivate)
                    {
                        child.gameObject.SetActive(shouldActivate);
                    }
                }
            }
            // ���� �ֱ⸦ �ø�
            yield return new WaitForSeconds(1f);
        }
    }

}
