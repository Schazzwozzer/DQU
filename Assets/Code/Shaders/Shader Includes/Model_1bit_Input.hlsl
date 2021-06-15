#ifndef DQU_MODEL_1BIT_INPUT_INCLUDED
#define DQU_MODEL_1BIT_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _BaseMap_ST;
    half4 _BaseColor;
    //half4 _SpecColor;
    half _Cutoff;
    half _Surface;
    // DQU properties
    half _HasColor2;
    half _HasColor3;
    half _HasColor4;
    half3 _Color1High;
    half3 _Color1Low;
#if defined(_PALETTE_COUNT_4) || defined(_PALETTE_COUNT_3) || defined(_PALETTE_COUNT_2)
    half3 _Color2High;
    half3 _Color2Low;
#endif
#if defined(_PALETTE_COUNT_4) || defined(_PALETTE_COUNT_3)
    half3 _Color3High;
    half3 _Color3Low;
#endif
#if defined(_PALETTE_COUNT_4)
    half3 _Color4High;
    half3 _Color4Low;
#endif
    half _ColorStep;
    half3 _ColorShadow;
    half4 _Fresnel;
CBUFFER_END

#ifdef UNITY_DOTS_INSTANCING_ENABLED
    UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
        UNITY_DOTS_INSTANCED_PROP(float4, _BaseColor)
        UNITY_DOTS_INSTANCED_PROP(float4, _ColorHigh)
        UNITY_DOTS_INSTANCED_PROP(float4, _ColorLow)
        UNITY_DOTS_INSTANCED_PROP(float4, _ColorShadow)
        UNITY_DOTS_INSTANCED_PROP(float4, _SpecColor)
        UNITY_DOTS_INSTANCED_PROP(float , _Cutoff)
        UNITY_DOTS_INSTANCED_PROP(float , _Surface)
    UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)

    #define _BaseColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__BaseColor)
    #define _ColorHigh          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__ColorHigh)
    #define _ColorLow           UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__ColorLow)
    #define _ColorShadow        UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__ColorShadow)
    #define _SpecColor          UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float4 , Metadata__SpecColor)
    #define _Cutoff             UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__Cutoff)
    #define _Surface            UNITY_ACCESS_DOTS_INSTANCED_PROP_FROM_MACRO(float  , Metadata__Surface)
#endif

TEXTURE2D(_BaseMap);            SAMPLER(sampler_BaseMap);
TEXTURE2D(_SpecGlossMap);       SAMPLER(sampler_SpecGlossMap);

///////////////////////////////////////////////////////////////////////////////
//                      Material Property Helpers                            //
///////////////////////////////////////////////////////////////////////////////
half Alpha(half albedoAlpha, half4 color, half cutoff)
{
#if !defined(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A) && !defined(_GLOSSINESS_FROM_BASE_ALPHA)
    half alpha = albedoAlpha * color.a;
#else
    half alpha = color.a;
#endif

#if defined(_ALPHATEST_ON)
    clip(alpha - cutoff);
#endif

    return alpha;
}

half4 SampleAlbedoAlpha(float2 uv, TEXTURE2D_PARAM(albedoAlphaMap, sampler_albedoAlphaMap))
{
    return SAMPLE_TEXTURE2D(albedoAlphaMap, sampler_albedoAlphaMap, uv);
}

half3 SampleNormal(float2 uv, TEXTURE2D_PARAM(bumpMap, sampler_bumpMap), half scale = 1.0h)
{
#ifdef _NORMALMAP
    half4 n = SAMPLE_TEXTURE2D(bumpMap, sampler_bumpMap, uv);
    #if BUMP_SCALE_NOT_SUPPORTED
        return UnpackNormal(n);
    #else
        return UnpackNormalScale(n, scale);
    #endif
#else
    return half3(0.0h, 0.0h, 1.0h);
#endif
}
/*
half4 SampleSpecularSmoothness(half2 uv, half alpha, half4 specColor, TEXTURE2D_PARAM(specMap, sampler_specMap))
{
    half4 specularSmoothness = half4(0.0h, 0.0h, 0.0h, 1.0h);
#ifdef _SPECGLOSSMAP
    specularSmoothness = SAMPLE_TEXTURE2D(specMap, sampler_specMap, uv) * specColor;
#elif defined(_SPECULAR_COLOR)
    specularSmoothness = specColor;
#endif

#ifdef _GLOSSINESS_FROM_BASE_ALPHA
    specularSmoothness.a = exp2(10 * alpha + 1);
#else
    specularSmoothness.a = exp2(10 * specularSmoothness.a + 1);
#endif

    return specularSmoothness;
}

inline void InitializeSimpleLitSurfaceData(float2 uv, out SurfaceData outSurfaceData)
{
    outSurfaceData = (SurfaceData)0;

    half4 albedoAlpha = SampleAlbedoAlpha(uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
    outSurfaceData.alpha = albedoAlpha.a;
    AlphaDiscard(outSurfaceData.alpha, _Cutoff);

    outSurfaceData.albedo = albedoAlpha.rgb;
#ifdef _ALPHAPREMULTIPLY_ON
    outSurfaceData.albedo *= outSurfaceData.alpha;
#endif

    half4 specularSmoothness = SampleSpecularSmoothness(uv, outSurfaceData.alpha, _SpecColor, TEXTURE2D_ARGS(_SpecGlossMap, sampler_SpecGlossMap));
    outSurfaceData.metallic = 0.0; // unused
    outSurfaceData.specular = specularSmoothness.rgb;
    outSurfaceData.smoothness = specularSmoothness.a;
    outSurfaceData.normalTS = SampleNormal(uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap));
    outSurfaceData.occlusion = 1.0; // unused
    outSurfaceData.emission = 0.0; //SampleEmission(uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
}
*/
#endif
