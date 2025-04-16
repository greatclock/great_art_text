Shader "Hidden/TextToRenderTexture/RenderShader_Orthographic3D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            Name "Orthographic3D"

            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            float4 _MainTex_TexelSize;
            
            float4 _3DEndPoint;

            float4 _3DFaded;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = float4(0, 0, 0, 0);
                float2 dir = normalize(_3DEndPoint.xy);
                float j;
                float invlen = 1 / _3DEndPoint.w;
                for (j = 0; j < _3DEndPoint.w; j += 0.5) {
                    float d = j + 0.5;
                    float4 t = tex2D(_MainTex, i.uv.xy + dir * d * _MainTex_TexelSize.xy);
                    if (t.a > color.a) {
                        color = t;
                        color.rgb = lerp(color.rgb, _3DFaded.rgb, d * _3DFaded.a * invlen);
                    }
                }
                return color;
            }
            ENDCG
        }

    }

}
