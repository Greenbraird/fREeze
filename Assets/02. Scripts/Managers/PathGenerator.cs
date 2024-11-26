using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public List<GameObject> pathPrefabs; // 길 프리팹 리스트
    public int initialPathCount = 15; // 초기 길 개수
    public float spawnDistance = 50f; // 플레이어와의 거리 기준으로 새 길 생성

    private List<GameObject> activePaths = new List<GameObject>();
    private Vector3 lastEndPivotPosition;
    private Quaternion lastEndPivotRotation;

    void Start()
    {
        // 시작 위치 초기화
        lastEndPivotPosition = Vector3.zero;
        lastEndPivotRotation = Quaternion.identity;

        // 초기 길 생성
        for (int i = 0; i < initialPathCount; i++)
        {
            SpawnPath();
        }
    }

    void Update()
    {
        // 플레이어가 새 길을 생성할 거리로 이동했는지 확인
        if (Vector3.Distance(player.position, lastEndPivotPosition) < spawnDistance)
        {
            SpawnPath();
            RemoveOldPath();
        }
    }

    private void SpawnPath()
    {
        // 랜덤 프리팹 선택
        GameObject prefab = pathPrefabs[Random.Range(0, pathPrefabs.Count)];
        GameObject newPath = Instantiate(prefab);

        // End_Pivot 위치와 회전을 기준으로 Start_Pivot 배치
        Transform startPivot = newPath.transform.Find("Start_Pivot");
        newPath.transform.position = lastEndPivotPosition;
        newPath.transform.rotation = lastEndPivotRotation;

        // Start_Pivot이 기준점에서 맞춰지도록 이동
        Vector3 positionOffset = newPath.transform.position - startPivot.position;
        newPath.transform.position += positionOffset;

        // 다음 End_Pivot 위치 및 회전값 갱신
        Transform endPivot = newPath.transform.Find("End_Pivot");
        lastEndPivotPosition = endPivot.position;
        lastEndPivotRotation = endPivot.rotation;

        activePaths.Add(newPath);
    }

    private void RemoveOldPath()
    {
        // N번째 길이 생성되었을 때 N-2번째 길을 삭제
        if (activePaths.Count > initialPathCount)
        {
            GameObject oldPath = activePaths[0]; // 첫 번째 길(N-2번째 길)
            activePaths.RemoveAt(0); // 리스트에서 제거
            Destroy(oldPath); // 게임 오브젝트 삭제
        }
    }
}
