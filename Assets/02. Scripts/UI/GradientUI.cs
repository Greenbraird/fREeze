using UnityEngine;
using UnityEngine.UI;

public class GradientUI : MonoBehaviour
{
    private Image uiImage;  // UI Image에 그라디언트를 적용할 대상
    public Color topColor = Color.white;  // 위쪽 색상
    public Color bottomColor = Color.black;  // 아래쪽 색상

    void Start()
    {
        uiImage = GetComponent<Image>();

        // Image의 Material을 가져옴
        Material mat = new Material(Shader.Find("Custom/GradientShader"));

        // 색상 설정
        mat.SetColor("_ColorTop", topColor);
        mat.SetColor("_ColorBottom", bottomColor);

        // UI Image에 Material 적용
        uiImage.material = mat;
    }
}
