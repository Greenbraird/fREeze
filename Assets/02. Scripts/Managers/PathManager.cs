using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Transform player; // �÷��̾� Transform
    public List<GameObject> pathPrefabs; // �� ������ ����Ʈ
    public GameObject initialPathPrefab; // ù ��° �� ������
    public float spawnDistance = 50f; // �÷��̾���� �Ÿ� �������� �� �� ����

    private List<GameObject> activePaths = new List<GameObject>();
    private Vector3 lastEndPivotPosition;
    private Quaternion lastEndPivotRotation;

    void Start()
    {
        // ù ��° ���� �ʱ� ��ġ�� ��ġ
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
        // �÷��̾ �� ���� ������ �Ÿ��� �̵��ߴ��� Ȯ��
        if (Vector3.Distance(player.position, lastEndPivotPosition) < spawnDistance)
        {
            SpawnPath();
            RemoveOldPath();
        }
    }

    private void SpawnPath()
    {
        // ���� ������ ����
        GameObject prefab = pathPrefabs[Random.Range(0, pathPrefabs.Count)];
        GameObject newPath = Instantiate(prefab);

        // End_Pivot ��ġ�� ȸ���� �������� Start_Pivot ��ġ
        Transform startPivot = newPath.transform.Find("Start_Pivot");
        newPath.transform.position = lastEndPivotPosition;
        newPath.transform.rotation = lastEndPivotRotation;

        // Start_Pivot�� ���������� ���������� �̵�
        Vector3 positionOffset = newPath.transform.position - startPivot.position;
        newPath.transform.position += positionOffset;

        // ���� End_Pivot ��ġ �� ȸ���� ����
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
