using UnityEngine;

public class PlayBoxRaycast : MonoBehaviour
{
    public string targetTag = "TargetTag"; // 찾고자 하는 Tag
    public LayerMask targetLayer; // Raycast가 감지할 레이어

    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0)) // 0은 왼쪽 마우스 버튼
        {
            RaycastHit hit;

            // 카메라에서 마우스 위치로 Ray 발사
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Raycast가 물체를 충돌하는지 확인
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayer))
            {
                // 충돌한 물체의 Tag가 targetTag와 일치하는지 확인
                if (hit.collider.CompareTag(targetTag))
                {
                    // 원하는 물체를 발견한 경우 처리
                    Debug.Log("Raycast hit an object with the tag: " + targetTag);
                    // 추가 처리 로직을 여기에 작성
                    LoadingSceneController.LoadSceneMode("3. Game");
                }
            }
        }
    }
}
