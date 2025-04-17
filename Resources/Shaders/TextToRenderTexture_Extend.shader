Shader "Hidden/TextToRenderTexture/RenderShader_Extend"
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
            Name "ExtendWithOffset"
            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            int _ExtendCount;
            float4 _Offsets[32];
            float4 _ExtendColorMask;

            fixed4 frag(v2f i) : SV_Target
            {
                int j;
                float4 origin = tex2D(_MainTex, i.uv);
#ifndef UNITY_COLORSPACE_GAMMA
                origin.rgb = LinearToGammaSpace(origin.rgb);
#endif
                float4 color = origin;
                for (j = 0; j < _ExtendCount; j++) {
                    float4 offset = _Offsets[j];
                    float4 c = sign(tex2D(_MainTex, i.uv + offset.xy) - offset.z) * offset.w;
                    color = max(c, color);
                }
                color = lerp(origin, color, _ExtendColorMask);
#ifndef UNITY_COLORSPACE_GAMMA
                color.rgb = GammaToLinearSpace(color.rgb);
#endif
                return color;
            }
            ENDCG
        }

    }

}
