using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadUpdate : MonoBehaviour
{
    public Transform player; // 플레이어(또는 카메라) 위치
    public float renderDistance = 150f; // 렌더링 활성화 거리
    public GameObject[] prefabParents; // 관리할 오브젝트 배열

    private void Start()
    {
        // 모든 오브젝트를 관리 대상으로 설정
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

                // 프리팹의 자식 순회
                foreach (Transform child in prefabParent.transform)
                {
                    if (child == null) continue;

                    // 거리 계산
                    float distance = Vector3.Distance(player.position, child.position);

                    // 거리 조건에 따라 활성화/비활성화
                    bool shouldActivate = distance <= renderDistance;
                    if (child.gameObject.activeSelf != shouldActivate)
                    {
                        child.gameObject.SetActive(shouldActivate);
                    }
                }
            }
            // 실행 주기를 늘림
            yield return new WaitForSeconds(1f);
        }
    }

}
