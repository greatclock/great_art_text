Shader "Hidden/TextToRenderTexture/RenderShader_Offset"
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
            Name "ContentAndOffset"
            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            int _Offsets1Count;
            int _Offsets2Count;
            float4 _Offsets[16];

            sampler2D _Text3D;

            fixed4 frag(v2f i) : SV_Target
            {
                int j;
                float outer = 0;
                for (j = 0; j < _Offsets1Count; j++) {
                    outer += tex2D(_MainTex, i.uv - _Offsets[j].xy).a;
                }
                float glow = 0;
                for (j = 0; j < _Offsets2Count; j++) {
                    glow += tex2D(_MainTex, i.uv - _Offsets[j].zw).a;
                }
                float val = tex2D(_MainTex, i.uv).a;
                float v3d = tex2D(_Text3D, i.uv).a;
                float4 color = float4(v3d, saturate(outer), 1 - sign(val), saturate(glow));
#ifndef UNITY_COLORSPACE_GAMMA
                color.rgb = GammaToLinearSpace(color.rgb);
#endif
                return color;
            }
            ENDCG
        }

    }

}
