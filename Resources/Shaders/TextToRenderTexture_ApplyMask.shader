Shader "Hidden/TextToRenderTexture/RenderShader_ApplyMask"
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
            Name "ApplyMask"

            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _TextTex;

            float4 _ChannelMask;
            
            fixed4 frag(v2f i) : SV_Target
            {
                float4 color = tex2D(_MainTex, i.uv.xy);
                float mask = sign(tex2D(_TextTex, i.uv.xy).a);
                color *= lerp(1, (1 - mask) * saturate(-_ChannelMask) + mask * saturate(_ChannelMask), abs(_ChannelMask));
#ifndef UNITY_COLORSPACE_GAMMA
                color.rgb = GammaToLinearSpace(color.rgb);
#endif
                return color;
            }
            ENDCG
        }

    }

}
