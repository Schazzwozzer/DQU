#ifndef DQU_SPRITE_PALETTE_INPUT_INCLUDED
#define DQU_SPRITE_PALETTE_INPUT_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

CBUFFER_START(UnityPerMaterial)
    half4 _MainTex_ST;
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

    half3 _ColorShadow = half3( 0.18, 0.2, 0.28 );
CBUFFER_END


TEXTURE2D(_MainTex);            SAMPLER(sampler_MainTex);
            


#endif