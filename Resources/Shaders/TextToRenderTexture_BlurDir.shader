Shader "Hidden/TextToRenderTexture/RenderShader_BlurDir"
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
            Name "BlurDir"

            CGPROGRAM
            #include "TextToRenderTexture.cginc"

            #pragma vertex vert
            #pragma fragment frag

            fixed4 frag(v2f i) : SV_Target
            {
                return fragBlur(i, _BlurDir, _BlurColorMask);
            }
            ENDCG
        }

    }

}
