Shader "Hidden/TextToRenderTexture/RenderShader_Blend"
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
            Name "Blend"

            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _TextTex;
            sampler2D _Text3D;
            
            sampler2D _InnerTex;
            float4 _InnerTex_ST;
            sampler2D _OuterTex;
            float4 _OuterTex_ST;
            sampler2D _OutlineTex;
            float4 _OutlineTex_ST;
            sampler2D _GlowTex;
            float4 _GlowTex_ST;
            
            int _InnerAlphaKeysCount;
            float4 _InnerAlphaKeys[8];
            int _InnerColorKeysCount;
            float4 _InnerColorKeys[8];

            int _OuterAlphaKeysCount;
            float4 _OuterAlphaKeys[8];
            int _OuterColorKeysCount;
            float4 _OuterColorKeys[8];

            int _OutlineAlphaKeysCount;
            float4 _OutlineAlphaKeys[8];
            int _OutlineColorKeysCount;
            float4 _OutlineColorKeys[8];

            float4 _InnerGradientDirection;
            float4 _OuterGradientDirection;
            float4 _OutlineGradientDirection;
            float4 _GlowGradientDirection;

            int _GlowAlphaKeysCount;
            float4 _GlowAlphaKeys[8];
            int _GlowColorKeysCount;
            float4 _GlowColorKeys[8];

            float4 _FxBlend;
            float4 _GlowBlend;

            float4 _Emboss;
            float4 _EmbossLightColor;

            float4 _MainTex_TexelSize;
            
            fixed4 frag(v2f i) : SV_Target
            {
                float gi = (dot(i.uv.zw, _InnerGradientDirection.xy) - _InnerGradientDirection.z) / (_InnerGradientDirection.w - _InnerGradientDirection.z);
                float go = (dot(i.uv.zw, _OuterGradientDirection.xy) - _OuterGradientDirection.z) / (_OuterGradientDirection.w - _OuterGradientDirection.z);
                float gl = (dot(i.uv.zw, _OutlineGradientDirection.xy) - _OutlineGradientDirection.z) / (_OutlineGradientDirection.w - _OutlineGradientDirection.z);
                float gg = (dot(i.uv.zw, _GlowGradientDirection.xy) - _GlowGradientDirection.z) / (_GlowGradientDirection.w - _GlowGradientDirection.z);
                float4 innerT = tex2D(_InnerTex, TRANSFORM_TEX(i.uv2.xy, _InnerTex));
                float4 outerT = tex2D(_OuterTex, TRANSFORM_TEX(i.uv.xy, _OuterTex));
                float4 outlineT = tex2D(_OutlineTex, TRANSFORM_TEX(i.uv3.xy, _OutlineTex));
                float4 glowT = tex2D(_GlowTex, TRANSFORM_TEX(i.uv.xy, _GlowTex));
                float4 txt = tex2D(_TextTex, i.uv.xy);
                float4 t3d = tex2D(_Text3D, i.uv.xy);
#ifndef UNITY_COLORSPACE_GAMMA
                innerT.rgb = LinearToGammaSpace(innerT.rgb);
                outerT.rgb = LinearToGammaSpace(outerT.rgb);
                outlineT.rgb = LinearToGammaSpace(outlineT.rgb);
                glowT.rgb = LinearToGammaSpace(glowT.rgb);
                txt.rgb = LinearToGammaSpace(txt.rgb);
                t3d.rgb = LinearToGammaSpace(t3d.rgb);
#endif
                float4 outerG = EvaluteGradient(_OuterAlphaKeysCount, _OuterAlphaKeys, _OuterColorKeysCount, _OuterColorKeys, go);
                float4 innerG = EvaluteGradient(_InnerAlphaKeysCount, _InnerAlphaKeys, _InnerColorKeysCount, _InnerColorKeys, gi);
                float4 outlineG = EvaluteGradient(_OutlineAlphaKeysCount, _OutlineAlphaKeys, _OutlineColorKeysCount, _OutlineColorKeys, gl);
                float4 glowG = EvaluteGradient(_GlowAlphaKeysCount, _GlowAlphaKeys, _GlowColorKeysCount, _GlowColorKeys, gg);
                
                float4 outineColor = outlineG * outlineT;
                float4 outerColor = outerG * outerT;
                float4 innerColor = innerG * innerT;
                float4 glowColor = glowG * glowT;
                
                float4 fx = tex2D(_MainTex, i.uv.xy);
                float4 txt_blend = lerp(float4(t3d.rgb * sign(1 - txt.a), t3d.a * (1 - txt.a)), AlphaBlend(t3d, txt, 0), _GlowBlend.z);
                float4 outer = float4(outerColor.rgb, outerColor.a * pow(fx.g, 1 / max(0.00001, _FxBlend.x)));
                float outlineAlpha = outineColor.a * pow(fx.r * 40, 2);
                float4 outline = float4(outineColor.rgb, step(0.1, outlineAlpha) * clamp(outlineAlpha, 0, outineColor.a));
                float4 inner = float4(innerColor.rgb, clamp(innerColor.a * pow(fx.b, _FxBlend.y) * txt.a * _GlowBlend.z, 0, innerColor.a));
                outline.a = lerp(outline.a * (1 - txt.a), outline.a, _GlowBlend.w);
                
                float emboss = 0;
                float2 duv = _MainTex_TexelSize.xy * 0.3;
                float b0 = tex2D(_MainTex, i.uv.xy + float2(-duv.x, -duv.y)).b;
                float b1 = tex2D(_MainTex, i.uv.xy + float2(-duv.x, duv.y)).b;
                float b2 = tex2D(_MainTex, i.uv.xy + float2(duv.x, duv.y)).b;
                float b3 = tex2D(_MainTex, i.uv.xy + float2(duv.x, -duv.y)).b;
                float ty1 = b1 - b0;
                float tx1 = b2 - b1;
                float ty2 = b2 - b3;
                float tx2 = b2 - b1;
                if (abs(ty1) + abs(tx1) > 0.0001 && abs(ty2) + abs(tx2) > 0.0001) {
                    float dir1 = atan2(ty1, tx1);
                    float dir2 = atan2(ty2, tx2);
                    if (isnan(dir1)) { dir1 = 0; }
                    if (isnan(dir2)) { dir2 = 0; }
                    float c1, s1, c2, s2;
                    sincos(dir1, s1, c1);
                    sincos(dir2, s2, c2);
                    float2 lightdir = _Emboss.zw;
                    emboss = dot(lightdir, float2(c1 + c2, s1 + s2) * 0.5) * (abs(b2 - b0) + abs(b3 - b1)) * _Emboss.y;
                }

                float4 blend = AlphaBlend(AlphaBlend(outer, outline, 0), txt_blend, 1);
                float4 innerBlend = AlphaBlend(blend, float4(inner.rgb, clamp(inner.a, 0, innerColor.a)), 0);

                float4 noglow = lerp(blend, innerBlend, _FxBlend.z) + float4(inner.rgb * inner.a * _FxBlend.w, 0);
                if (emboss > 0) {
                    noglow.rgb += _EmbossLightColor.rgb * (_EmbossLightColor.a * _Emboss.x * emboss);
                } else {
                    noglow.rgb = lerp(noglow.rgb, 0, -emboss * _Emboss.x);
                }
                float4 final = noglow + float4(glowColor.rgb * (glowColor.a * pow(fx.a, max(0.0001, _GlowBlend.x * _GlowBlend.x)) * _GlowBlend.y), 0);
#ifndef UNITY_COLORSPACE_GAMMA
                final.rgb = GammaToLinearSpace(final.rgb);
#endif
                return final;
            }
            ENDCG
        }

    }

}
