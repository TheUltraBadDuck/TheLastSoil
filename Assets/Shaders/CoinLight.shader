Shader "Unlit/CoinLight"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Cull Off
        Blend One OneMinusSrcAlpha

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
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half4 _Color;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 textureColor = tex2D(_MainTex, i.uv);
                textureColor.rgb *= textureColor.a;
                for (int k = 1; k < 9; k += 2) {
                    float2 newUV = i.uv + float2(k % 3 - 1, k / 3 - 1) / 32.0;

                    float4 newBackground = tex2D(_MainTex, newUV);
                    newBackground.rgb = _Color.rgb;
                    newBackground.rgb *= newBackground.a;

                    if (textureColor.a == 0)
                        textureColor += newBackground;
                }
                return textureColor;
            }
            ENDCG
        }
    }
}
