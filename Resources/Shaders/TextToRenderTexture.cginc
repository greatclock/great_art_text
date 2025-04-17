#ifndef GREATARTTEXT_TEXTTORENDERTEXTURE
#define GREATARTTEXT_TEXTTORENDERTEXTURE

#include "UnityCG.cginc"
    
struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float4 uv : TEXCOORD0;
    float4 uv2 : TEXCOORD1;
    float4 uv3 : TEXCOORD2;
};

float4 _Size;
float4 _RegionText;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv.xy = v.uv;
    o.uv.zw = v.uv * _Size.xy;
    o.uv2.zw = o.uv.zw - _RegionText.xy;
    o.uv2.xy = o.uv2.zw / _RegionText.zw;
    o.uv3.zw = o.uv2.zw - _Size.zw;
    o.uv3.xy = o.uv3.zw / (_RegionText.zw + _Size.zw + _Size.zw);
    return o;
}
    
sampler2D _MainTex;
float2 _BlurDir;
float4 _BlurColorMask;
float _ColorLevelMin;

float4 fragBlur(v2f i, float2 dir, float4 mask)
{
    float2 uv = i.uv;
    float2 d = dir * 0.3333333;
    float4 col = float4(0, 0, 0, 0);
    float4 tex = tex2D(_MainTex, uv);
    col += 0.324 * tex;
    col += 0.232 * tex2D(_MainTex, uv + d);
    col += 0.232 * tex2D(_MainTex, uv - d);
    col += 0.0855 * tex2D(_MainTex, uv + d * 2);
    col += 0.0855 * tex2D(_MainTex, uv - d * 2);
    col += 0.0205 * tex2D(_MainTex, uv + d * 3);
    col += 0.0205 * tex2D(_MainTex, uv - d * 3);
    return lerp(tex, col * max(mask * (1 - step(mask, 1)), 1) * step(_ColorLevelMin, col), saturate(mask));
}

float4 fragBlurHalf(v2f i, float2 dir, float4 mask)
{
    float2 uv = i.uv;
    float2 d = dir * 0.3333333;
    float4 col = float4(0, 0, 0, 0);
    float4 tex = tex2D(_MainTex, uv);
    col += 0.489 * tex;
    col += 0.351 * tex2D(_MainTex, uv + d);
    col += 0.129 * tex2D(_MainTex, uv + d * 2);
    col += 0.031 * tex2D(_MainTex, uv + d * 3);
    return lerp(tex, col * max(mask * (1 - step(mask, 1)), 1) * step(_ColorLevelMin, col), saturate(mask));
}

float4 AlphaBlend(float4 dst, float4 src, float src2dstWhen0)
{
    float alpha = 1 - (1 - src.a) * (1 - dst.a);
    if (alpha <= 0) { return float4(lerp(src.rgb, dst.rgb, src2dstWhen0), 0); }
    return float4((dst.rgb * dst.a * (1 - src.a) + src.rgb * src.a) / alpha, alpha);
}

float4 EvaluteGradient(int alphaKeysCount, float4 alphaKeys[8], int colorKeysCount, float4 colorKeys[8], float t) {
    float4 color = float4(colorKeys[0].rgb, alphaKeys[0].r);
    int j;
    for (j = 0; j < alphaKeysCount - 1; j++) {
        float4 cur = alphaKeys[j];
        if (t > cur.a) {
            float4 next = alphaKeys[j + 1];
            color.a = lerp(cur.r, next.r, saturate((t - cur.a) / (next.a - cur.a)));
        }
    }
    for (j = 0; j < colorKeysCount - 1; j++) {
        float4 cur = colorKeys[j];
        if (t > cur.a) {
            float4 next = colorKeys[j + 1];
            color.rgb = lerp(cur.rgb, next.rgb, saturate((t - cur.a) / (next.a - cur.a)));
        }
    }
    return color;
}

#endif