Shader "Custom/GradientAlphaShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // 텍스처 프로퍼티
        _ColorTop ("Top Color", Color) = (1, 1, 1, 1)  // 위쪽 색상
        _ColorBottom ("Bottom Color", Color) = (1, 1, 1, 1)  // 아래쪽 색상
        _AlphaStrength ("Alpha Strength", Range(0, 1)) = 0.5  // 알파 값의 강도
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
                // 텍스처 색상 샘플링
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // 그라디언트 색상 계산 (y축 기준으로)
                fixed4 gradientColor = lerp(_ColorBottom, _ColorTop, i.uv.y);

                // 중앙에서 알파 값을 낮추기 위한 거리 계산 (중앙점은 UV 좌표의 (0.5, 0.5))
                float2 center = float2(0.5, 0.5);
                float distanceToCenter = distance(i.uv, center);

                // 알파 값 계산: 거리가 멀수록 알파 값이 커짐 (최대값을 _AlphaStrength로 설정)
                float alpha = 1.0 - saturate(distanceToCenter / _AlphaStrength);

                // 최종 색상은 텍스처와 그라디언트 색상에 알파 값 적용
                gradientColor.a *= alpha;
                return texColor * gradientColor;
            }
            ENDCG
        }
    }
}
