Shader "UI/RadialFadeInward"
{
    Properties
    {
        _MainTex("Unused", 2D) = "white" {}
        _Color("Fade Color", Color) = (0,0,0,1)
        _Fade("Fade", Range(0,2)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Overlay+1" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Color;
            float _Fade;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.uv - float2(0.5, 0.5));
                float alpha = smoothstep(0.0, _Fade, dist); // fade จากขอบเข้า
                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}