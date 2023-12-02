Shader "Unlit/Fade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Offset("Offset", Float) = 1 
        _Color("Color", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Offset;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half2 _vec = i.uv - 0.5f;
                half _magnitude = sqrt( _vec.x * _vec.x + _vec.y * _vec.y);
                half _step = 1 - step(_magnitude, _Offset);
                return _step * _Color;
            }
            ENDCG
        }
    }
}
