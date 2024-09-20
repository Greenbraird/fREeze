Shader "Custom/GradientShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // 텍스처 프로퍼티 추가
        _ColorTop ("Top Color", Color) = (1, 1, 1, 1)  // 위쪽 색상
        _ColorBottom ("Bottom Color", Color) = (0, 0, 0, 1)  // 아래쪽 색상
        _StencilComp ("Stencil Comparison", Float) = 8  // 스텐실 비교
        _Stencil ("Stencil ID", Float) = 0  // 스텐실 ID
        _StencilOp ("Stencil Operation", Float) = 0  // 스텐실 연산
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        // 스텐실 설정
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

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
            float4 _MainTex_ST;
            fixed4 _ColorTop;
            fixed4 _ColorBottom;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 텍스처 색상 샘플링
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // 그라디언트 계산 (y축 기준으로)
                fixed4 gradientColor = lerp(_ColorBottom, _ColorTop, i.uv.y);

                // 텍스처와 그라디언트 색상을 곱함
                return texColor * gradientColor;
            }
            ENDCG
        }
    }
}
