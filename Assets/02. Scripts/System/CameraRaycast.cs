using UnityEngine;

public class PlayBoxRaycast : MonoBehaviour
{
    public string targetTag = "TargetTag"; // ã���� �ϴ� Tag
    public LayerMask targetLayer; // Raycast�� ������ ���̾�

    void Update()
    {
        // ���콺 ���� ��ư Ŭ�� ����
        if (Input.GetMouseButtonDown(0)) // 0�� ���� ���콺 ��ư
        {
            RaycastHit hit;

            // ī�޶󿡼� ���콺 ��ġ�� Ray �߻�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast�� ��ü�� �浹�ϴ��� Ȯ��
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
            {
                // �浹�� ��ü�� Tag�� targetTag�� ��ġ�ϴ��� Ȯ��
                if (hit.collider.CompareTag(targetTag))
                {
                    // ���ϴ� ��ü�� �߰��� ��� ó��
                    Debug.Log("Raycast hit an object with the tag: " + targetTag);
                    // �߰� ó�� ������ ���⿡ �ۼ�
                    LoadingSceneController.LoadSceneMode("3. Game");
                }
            }
        }
    }
}
