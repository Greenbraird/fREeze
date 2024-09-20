Shader "Custom/GradientShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // �ؽ�ó ������Ƽ �߰�
        _ColorTop ("Top Color", Color) = (1, 1, 1, 1)  // ���� ����
        _ColorBottom ("Bottom Color", Color) = (0, 0, 0, 1)  // �Ʒ��� ����
        _StencilComp ("Stencil Comparison", Float) = 8  // ���ٽ� ��
        _Stencil ("Stencil ID", Float) = 0  // ���ٽ� ID
        _StencilOp ("Stencil Operation", Float) = 0  // ���ٽ� ����
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

        // ���ٽ� ����
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
                // �ؽ�ó ���� ���ø�
                fixed4 texColor = tex2D(_MainTex, i.uv);

                // �׶���Ʈ ��� (y�� ��������)
                fixed4 gradientColor = lerp(_ColorBottom, _ColorTop, i.uv.y);

                // �ؽ�ó�� �׶���Ʈ ������ ����
                return texColor * gradientColor;
            }
            ENDCG
        }
    }
}
