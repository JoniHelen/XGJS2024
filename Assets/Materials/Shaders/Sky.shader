Shader "Custom/Sky"
{
    Properties
    {
        _SkyColor("Sky Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _GroundColor("Ground Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _HorizonColor("Horizon Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SunSize("Sun Size", Float) = 1.0
        _SunFalloff("Sun Falloff", Float) = 0.1
        _SunIntensity("Sun Intensity", Float) = 1
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

            #include "Environment.hlsl"
            
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
                float3 positionWS : TEXCOORD1;
            };

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.poisitonOS.xyz);
                output.uv = input.uv;
                output.positionWS = TransformObjectToWorld(input.poisitonOS.xyz);
                return output;
            }

            CBUFFER_START(UnityPerMaterial)
            float4 _SkyColor;
            float4 _GroundColor;
            float4 _HorizonColor;
            float _SunSize;
            float _SunFalloff;
            float _SunIntensity;
            CBUFFER_END

            float4 Fragment(Varyings input) : SV_TARGET
            {
                Sky sky;
                sky.SkyColor = _SkyColor;
                sky.GroundColor = _GroundColor;
                sky.HorizonColor = _HorizonColor;
                sky.SunSize = _SunSize;
                sky.SunFalloff = _SunFalloff;
                sky.SunIntensity = _SunIntensity;
                
                return SampleEnvironment(-GetWorldSpaceNormalizeViewDir(input.positionWS), sky);
            }
            
            ENDHLSL
        }
    }
}
