Shader "Hidden/TextToRenderTexture/RenderShader_Edge"
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
            Name "Edge"

            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _TextTex;

            float4 _MainTex_TexelSize;
            
            int _Light360AlphaKeysCount;
            float4 _Light360AlphaKeys[8];
            int _Light360ColorKeysCount;
            float4 _Light360ColorKeys[8];

            float _Phase;

            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_TextTex, i.uv.xy);
                float2 duv = _MainTex_TexelSize.xy * 0.3;
                float b0 = tex2D(_MainTex, i.uv.xy + float2(-duv.x, -duv.y)).a;
                float b1 = tex2D(_MainTex, i.uv.xy + float2(-duv.x, duv.y)).a;
                float b2 = tex2D(_MainTex, i.uv.xy + float2(duv.x, duv.y)).a;
                float b3 = tex2D(_MainTex, i.uv.xy + float2(duv.x, -duv.y)).a;
                float ty1 = b1 - b0;
                float tx1 = b2 - b1;
                float ty2 = b2 - b3;
                float tx2 = b2 - b1;
                if (abs(ty1) + abs(tx1) > 0.0001 && abs(ty2) + abs(tx2) > 0.0001) {
                    float dir1 = atan2(ty1, tx1);
                    float dir2 = atan2(ty2, tx2);
                    float c1, s1, c2, s2;
                    sincos(dir1, s1, c1);
                    sincos(dir2, s2, c2);

                    if (abs(dir1 - dir2) > UNITY_PI) {
                        if (dir1 < dir2) {
                            dir1 += UNITY_TWO_PI;
                        } else {
                            dir2 += UNITY_TWO_PI;
                        }
                    }
                    float phase = ((dir1 + dir2) * 0.5 - _Phase + UNITY_PI) * UNITY_INV_TWO_PI;
                    float4 light = EvaluteGradient(_Light360AlphaKeysCount, _Light360AlphaKeys, _Light360ColorKeysCount, _Light360ColorKeys, frac(phase));
                    if (light.a > 0.5) {
                        color.rgb = 1 - 2 * (1 - color.rgb) * (1 - light.a);
                    } else {
                        color.rgb *= 2 * light.a;
                    }
                    float3 steps = step(light.rgb, 0.5);
                    color.rgb = 1 - (1 - color.rgb) * (1 - light.rgb);
                }
#ifndef UNITY_COLORSPACE_GAMMA
                color.rgb = GammaToLinearSpace(color.rgb);
#endif
                return color;
            }
            ENDCG
        }

    }

}
