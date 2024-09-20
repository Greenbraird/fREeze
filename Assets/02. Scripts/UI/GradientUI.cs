using UnityEngine;
using UnityEngine.UI;

public class GradientUI : MonoBehaviour
{
    private Image uiImage;  // UI Image�� �׶���Ʈ�� ������ ���
    public Color topColor = Color.white;  // ���� ����
    public Color bottomColor = Color.black;  // �Ʒ��� ����

    void Start()
    {
        uiImage = GetComponent<Image>();

        // Image�� Material�� ������
        Material mat = new Material(Shader.Find("Custom/GradientShader"));

        // ���� ����
        mat.SetColor("_ColorTop", topColor);
        mat.SetColor("_ColorBottom", bottomColor);

        // UI Image�� Material ����
        uiImage.material = mat;
    }
}
