#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct Sky
{
    float4 SkyColor;
    float4 HorizonColor;
    float4 GroundColor;
    float SunSize;
    float SunFalloff;
    float SunIntensity;
};

float4 SampleEnvironment(float3 direction, Sky sky)
{
    Light mainLight = GetMainLight();
    
    float cosAlpha = dot(direction, float3(0.0f, 1.0f, 0.0f));
    float skyGradient = smoothstep(0.0f, 0.2f, saturate(cosAlpha));
    float groundGradient = smoothstep(0.0f, 0.01f, saturate(-cosAlpha));

    float3 sun = smoothstep(0.5f * sky.SunSize + sky.SunFalloff, 0.5f * sky.SunSize, degrees(acos(dot(direction, mainLight.direction)))) * sky.SunIntensity * mainLight.color;
    
    float4 skyCol = lerp(sky.HorizonColor, sky.SkyColor, skyGradient) * step(0.0f, cosAlpha);
    float4 groundCol = lerp(sky.HorizonColor, sky.GroundColor, groundGradient) * step(0.0f, -cosAlpha);
    return skyCol + groundCol + float4(sun, 1.0f);
}