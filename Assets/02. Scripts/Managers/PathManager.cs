using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Transform player; // 플레이어 Transform
    public List<GameObject> pathPrefabs; // 길 프리팹 리스트
    public GameObject initialPathPrefab; // 첫 번째 길 프리팹
    public float spawnDistance = 50f; // 플레이어와의 거리 기준으로 새 길 생성

    private List<GameObject> activePaths = new List<GameObject>();
    private Vector3 lastEndPivotPosition;
    private Quaternion lastEndPivotRotation;

    void Start()
    {
        // 첫 번째 길을 초기 위치에 배치
        if (initialPathPrefab != null)
        {
            GameObject initialPath = Instantiate(initialPathPrefab);
            activePaths.Add(initialPath);
            lastEndPivotPosition = initialPath.transform.Find("End_Pivot").position;
            lastEndPivotRotation = initialPath.transform.Find("End_Pivot").rotation;
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
        if (activePaths.Count > 1)
        {
            GameObject oldPath = activePaths[0];
            activePaths.RemoveAt(0);
            Destroy(oldPath);
        }
    }
}
