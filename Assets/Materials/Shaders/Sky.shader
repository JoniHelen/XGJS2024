Shader "Custom/Sky"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        
        Pass
        {
            Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
            ZWrite Off
            Cull Off
            
            HLSLPROGRAM

            #pragma vertex Vertex;
            #pragma fragment Fragment;

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 poisitonOS : POSITION;
                float3 normalOS : NORMAL;
                float3 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vertex(Attributes input) {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.poisitonOS.xyz);
                output.uv = input.uv;

                return output;
            }

            float4 Fragment(Varyings input) : SV_TARGET {
                return 0;
            }
            
            ENDHLSL
        }
    }
}
