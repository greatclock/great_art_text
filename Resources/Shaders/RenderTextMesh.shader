Shader "Hidden/TextToRenderTexture/RenderTextMesh"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
        Cull Off
        LOD 100
        ZTest Always
        BlendOp Max

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                float4 uv2 : TEXCOORD1;
                float4 uv3 : TEXCOORD2;
                float4 uv4 : TEXCOORD3;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 uv : TEXCOORD0;
                float4 uv2 : TEXCOORD1;
                float4 uv3 : TEXCOORD2;
                float4 uv4 : TEXCOORD3;
                float4 color : COLOR;
            };
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv2 = v.uv2;
                o.uv3 = v.uv3;
                o.uv4 = v.uv4;
                o.color = v.color;
                return o;
            }

            sampler2D _MainTex;
            float4 _TextRegion;
            
            float3 _Trapezoid;

            float4 text_tex(sampler2D tex, float2 coord, float2 uvLB, float2 uvLT, float2 uvRT, float2 uvRB)
            {
                float2 uv = coord;
                float p = _Trapezoid.z + 1;
                if (_Trapezoid.z < 0) { p = 1 / (1 - _Trapezoid.z); }
                if (_Trapezoid.x > 0) {
                    float temp = abs(_Trapezoid.y);
                    if (_Trapezoid.y > 0) {
                        temp *= pow(coord.y, p);
                    } else {
                        temp *= pow(1 - coord.y, p);
                    }
                    uv.x = (2 * coord.x - temp) / (2 - 2 * temp);
                } else if (_Trapezoid.x < 0) {
                    float temp = abs(_Trapezoid.y);
                    if (_Trapezoid.y > 0) {
                        temp *= pow(coord.x, p);
                    } else {
                        temp *= pow(1 - coord.x, p);
                    }
                    uv.y = (2 * coord.y - temp) / (2 - 2 * temp);
                }
                float valid = saturate(sign(0.5 - abs(uv.x - 0.5)) + 1) * saturate(sign(0.5 - abs(uv.y - 0.5)) + 1);
                uv = saturate(uv);
                uv = lerp(lerp(uvLB, uvRB, uv.x), lerp(uvLT, uvRT, uv.x), uv.y) + lerp(lerp(uvLB, uvLT, uv.y), lerp(uvRB, uvRT, uv.y), uv.x);
                float4 color = tex2D(tex, uv * 0.5) * valid;
#ifndef UNITY_COLORSPACE_GAMMA
                color.rgb = LinearToGammaSpace(color.rgb);
#endif
                return color;
            }

            float4 get_color_from_mode(float alpha, float4 base_color, float4 vert_color, float mode)
            {
                float4 color = float4(0, 0, 0, 0);
                color += base_color * saturate(1 - abs(mode - 0));
                color += vert_color * saturate(1 - abs(mode - 1));
                color += base_color * vert_color * saturate(1 - abs(mode - 2));
                color += float4((base_color.rgb + vert_color.rgb) * saturate(1 - abs(mode - 3)), base_color.a);
                color += float4((base_color.rgb - vert_color.rgb) * saturate(1 - abs(mode - 4)), base_color.a);
                float fa = alpha * color.a;
                return float4(color.rgb, fa);
            }
            float4 _PerCharPattern;

            sampler2D _PatternTex;
            float4 _PatternTex_ST;

            int _AlphaKeysCount;
            float4 _AlphaKeys[8];
            int _ColorKeysCount;
            float4 _ColorKeys[8];
            float4 _GradientDirection;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 coord = (i.uv2.zw - _TextRegion.xy) / _TextRegion.zw;
                float2 colorUV = lerp(coord, i.uv2.xy, _PerCharPattern.y);
                float4 pattern = tex2D(_PatternTex, TRANSFORM_TEX(colorUV, _PatternTex));
#ifndef UNITY_COLORSPACE_GAMMA
                pattern.rgb = LinearToGammaSpace(pattern.rgb);
#endif
                float globalGradientRatio = (dot(i.uv2.zw, _GradientDirection.xy) - _GradientDirection.z) / (_GradientDirection.w - _GradientDirection.z);
                float gradientRatio = lerp(globalGradientRatio, i.uv.w, _PerCharPattern.x);
                float4 gradientColor = float4(_ColorKeys[0].rgb, _AlphaKeys[0].r);
                int j;
                for (j = 0; j < _AlphaKeysCount - 1; j++) {
                    float4 cur = _AlphaKeys[j];
                    if (gradientRatio > cur.a) {
                        float4 next = _AlphaKeys[j + 1];
                        gradientColor.a = lerp(cur.r, next.r, saturate((gradientRatio - cur.a) / (next.a - cur.a)));
                    }
                }
                for (j = 0; j < _ColorKeysCount - 1; j++) {
                    float4 cur = _ColorKeys[j];
                    if (gradientRatio > cur.a) {
                        float4 next = _ColorKeys[j + 1];
                        gradientColor.rgb = lerp(cur.rgb, next.rgb, saturate((gradientRatio - cur.a) / (next.a - cur.a)));
                    }
                }
                float alpha = text_tex(_MainTex, i.uv2.xy, i.uv3.xy, i.uv3.zw, i.uv4.xy, i.uv4.zw).a;
                float4 color = get_color_from_mode(alpha, pattern * gradientColor, i.color, i.uv.z);
                // color.rgb = 1;
#ifndef UNITY_COLORSPACE_GAMMA
                color.rgb = GammaToLinearSpace(color.rgb);
#endif
                return color;
            }
            ENDCG
        }

    }
}
