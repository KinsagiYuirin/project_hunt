Shader "CustomRenderTexture/ScreenEdgeRed"
{
    Properties
    {
        _MainTex("Unused", 2D) = "white" {}
        _EdgeWidth ("Edge Width", Range(0.0, 0.5)) = 0.1
        _EdgeColor ("Edge Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 screenUV : TEXCOORD0;
            };

            sampler2D _MainTex;
            float _EdgeWidth;
            float4 _EdgeColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenUV = o.pos.xy / o.pos.w;
                o.screenUV = (o.screenUV + 1.0) * 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.screenUV;

                // ระยะใกล้ขอบ (ใกล้ 0 หรือ 1)
                float edge = min(min(uv.x, 1.0 - uv.x), min(uv.y, 1.0 - uv.y));

                if (edge < _EdgeWidth)
                {
                    // ถ้าอยู่ในระยะขอบที่กำหนด → แสดงสีแดง
                    float alpha = smoothstep(_EdgeWidth, 0, edge);
                    return _EdgeColor * alpha;
                }

                // ไม่ติดขอบ → โปร่งใส
                return float4(0,0,0,0);
            }
            ENDCG
        }
    }
}
