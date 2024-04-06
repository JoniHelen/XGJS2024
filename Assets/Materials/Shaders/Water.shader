Shader "Custom/Water"
{
    Properties
    {
        _NormalMap("Normal Map", 2D) = "bump" {}
        _WaterColor("Water Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _WaterShallowColor("Water Shallow Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SkyColor("Sky Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _GroundColor("Ground Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _HorizonColor("Horizon Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _SunSize("Sun Size", Float) = 1.0
        _SunFalloff("Sun Falloff", Float) = 0.1
        _SunIntensity("Sun Intensity", Float) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "RenderPipeline"="UniversalPipeline" }
        
        Pass
        {
            Tags { "LightMode"="SRPDefaultUnlit" }
            ZTest LEqual
            ZWrite On
            Cull Back
            
            HLSLPROGRAM

            #pragma vertex Vertex;
            #pragma fragment Fragment;

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Environment.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"
            
            struct Attributes
            {
                float4 poisitonOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float3 tangentWS : TEXCOORD2;
                float3 normalWS : TEXCOORD3;
                float3 bitangentWS : TEXCOORD4;
            };

            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            CBUFFER_START(UnityPerMaterial)
            float4 _NormalMap_ST;
            float4 _SkyColor;
            float4 _GroundColor;
            float4 _HorizonColor;
            float4 _WaterColor;
            float4 _WaterShallowColor;
            float _SunSize;
            float _SunFalloff;
            float _SunIntensity;
            CBUFFER_END

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.poisitonOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _NormalMap);
                output.positionWS = TransformObjectToWorld(input.poisitonOS.xyz);

                VertexNormalInputs normal_inputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);
                output.normalWS = normal_inputs.normalWS;
                output.tangentWS = normal_inputs.tangentWS;
                output.bitangentWS = normal_inputs.bitangentWS;
                return output;
            }

            float3 SampleNormal(float2 uv)
            {
                return UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, uv), 1.0f);
            }

            float2 rotateUV(float2 uv, float rotation)
            {
                float mid = 0.5;
                return float2(
                    cos(rotation) * (uv.x - mid) + sin(rotation) * (uv.y - mid) + mid,
                    cos(rotation) * (uv.y - mid) - sin(rotation) * (uv.x - mid) + mid
                );
            }

            half ShadowAtten(float3 worldPosition)
            {
                    return MainLightRealtimeShadow(TransformWorldToShadowCoord(worldPosition));
            }

            float4 Fragment(Varyings input) : SV_TARGET
            {
                float3 normalTSLow = SampleNormal(input.positionWS.xz * 0.01f + _Time.x * 0.1f);
                float3 normalTSHigh = SampleNormal(rotateUV(input.positionWS.xz, PI) * 0.05f + float2(1.0f, -1.0f) * (_Time.x * 0.5f));

                float3 normalTS = BlendNormalRNM(normalTSLow, normalTSHigh);
                
                float3 normalWS = TransformTangentToWorldDir(normalTS, float3x3(input.tangentWS, input.bitangentWS, input.normalWS), true);

                Sky sky;
                sky.SkyColor = _SkyColor;
                sky.GroundColor = _GroundColor;
                sky.HorizonColor = _HorizonColor;
                sky.SunSize = _SunSize;
                sky.SunFalloff = _SunFalloff;
                sky.SunIntensity = _SunIntensity;

                float3 viewDir = GetWorldSpaceNormalizeViewDir(input.positionWS);

                float reflectivity = saturate(0.02037f + (1 - 0.02037f) * pow(1 - dot(normalWS, viewDir), 5.0f));

                float3 refl = reflect(-viewDir, normalWS);

                float2 screenSpaceUV = GetNormalizedScreenSpaceUV(input.positionHCS.xy);
                float sceneDepth = LinearEyeDepth(SampleSceneDepth(screenSpaceUV), _ZBufferParams);
                float surfaceDepth = LinearEyeDepth(input.positionWS, GetWorldToViewMatrix());

                float waterDepth = saturate(sceneDepth - surfaceDepth);

                float4 waterCol = lerp(float4(SampleSceneColor(screenSpaceUV), 1.0f), _WaterShallowColor, waterDepth);

                if (sceneDepth - surfaceDepth > 5.0f)
                {
                    waterCol = _WaterColor;
                }
                
                if (dot(refl, float3(0.0f, 1.0f, 0.0f)) < 0.0f)
                {
                    return waterCol;
                }
                
                return lerp(waterCol, SampleEnvironment(refl, sky) * ShadowAtten(input.positionWS), reflectivity);
            }
            
            ENDHLSL
        }
    }
}
