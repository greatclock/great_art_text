Shader "Hidden/TextToRenderTexture/RenderShader_Perspective3D"
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
            Name "Perspective3D"

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
                float2 dir = i.uv2.zw - _3DEndPoint.xy;
                    dir = normalize(dir);
                    int j;
                    for (j = 1; j <= _3DEndPoint.w; j++) {
                        float d = j * 0.5;
                        float dis = distance(i.uv2.zw + dir * d, _3DEndPoint.xy) * _3DEndPoint.z;
                        if (d <= dis) {
                            float4 t = tex2D(_MainTex, i.uv.xy + dir * d * _MainTex_TexelSize.xy);
                            if (t.a > color.a) {
                                color = t;
                                color.rgb = lerp(color.rgb, _3DFaded.rgb, _3DFaded.a * d / dis);
                            }
                        }
                    }
                return color;
            }
            ENDCG
        }

    }

}
