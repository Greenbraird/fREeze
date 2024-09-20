Shader "Custom/GradientAlphaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // �ؽ�ó ������Ƽ
        _ColorTop ("Top Color", Color) = (1, 1, 1, 1)  // ���� ����
        _ColorBottom ("Bottom Color", Color) = (1, 1, 1, 1)  // �Ʒ��� ����
        _AlphaStrength ("Alpha Strength", Range(0, 1)) = 0.5  // ���� ���� ����
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _ColorTop;
            fixed4 _ColorBottom;
            float _AlphaStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // �ؽ�ó ���� ���ø�
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // �׶���Ʈ ���� ��� (y�� ��������)
                fixed4 gradientColor = lerp(_ColorBottom, _ColorTop, i.uv.y);

                // �߾ӿ��� ���� ���� ���߱� ���� �Ÿ� ��� (�߾����� UV ��ǥ�� (0.5, 0.5))
                float2 center = float2(0.5, 0.5);
                float distanceToCenter = distance(i.uv, center);

                // ���� �� ���: �Ÿ��� �ּ��� ���� ���� Ŀ�� (�ִ밪�� _AlphaStrength�� ����)
                float alpha = 1.0 - saturate(distanceToCenter / _AlphaStrength);

                // ���� ������ �ؽ�ó�� �׶���Ʈ ���� ���� �� ����
                gradientColor.a *= alpha;
                return texColor * gradientColor;
            }
            ENDCG
        }
    }
}
