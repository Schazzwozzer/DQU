#ifndef DQU_SPRITE_PALETTE_FRAGMENT_INCLUDED
#define DQU_SPRITE_PALETTE_FRAGMENT_INCLUDED

#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/NoiseHelpers.hlsl"
#include "Assets/Code/Shaders/Library/PaletteColors.hlsl"
#include "Assets/Code/Shaders/Library/Official Edits/Lighting.hlsl"

uniform float _NoiseSize;
uniform float3 _NoiseOffset;


half4 SampleMainTexture( float2 uv )
{
    // Sample the main albedo/diffuse texture.
    // Red channel attenuates blending between the high and low color tones.
    // Green channel is a sort of ambient occlusion/shadow map.
    // Blue channel are color/tone masks.
    return SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, uv, 0);
}


// Determine the material's base color.
half3 CalculateAlbedo( half colorBlend, half colorMask, half ditherPattern )
{
    // Determine our two colors — the high tone and low tone.
    half3 lowColor, highColor;
    GetPaletteColors( colorMask, lowColor, highColor );

    // Convert the color blend to a 1-bit value.
    colorBlend = step( colorBlend, ditherPattern );

    return lerp( lowColor, highColor, colorBlend );
}



half4 FragmentProgram_Lit( Varyings i ) : SV_Target
{
    float2 screenPos = i.positionNDC.xy;
    // Sample the required dither patterns.
    half ditherScreen64 = ScreenDither64( screenPos );
    half ditherScreen4 = ScreenDither4( screenPos );
    half ditherWorld64 = WorldDither64( i.positionWS.xy, 32 );

    half4 main = SampleMainTexture( i.uv );

    // Start with the base color of the material.
    half3 albedo = CalculateAlbedo( main.r, main.b, ditherWorld64 );

#ifdef _RECEIVE_LIGHTING
    // Next up we'll incorporate lighting. Start by 
    // running the standard Unity lighting calculations.
    half lighting = CalculateShapeLightShared( screenPos );

    // Take that lighting, currently a smooth gradient, and divide it 
    // into three tones: unlit, fully lit, and partially lit. The tones
    // are blended using a dithering pattern.
    half ditheredLighting = 
        (step( ditherScreen4, smoothstep( 0.1, 0.3, lighting )) + 
         step( ditherScreen64, smoothstep( 0.6, 0.8, lighting ))) / 2;

    // I want lighting to attenuate or "blow out" the detail 
    // texture, so they're multiplied together, then dithered 
    // and added to the dithered lighting we just calculated.
    lighting = step( ditherScreen64, main.g * lighting ) * ditheredLighting;
#else
    half lighting = step( ditherScreen64, main.g );
#endif

    // Combine albedo with lighting/shadows for final color.
    half3 color = lerp( albedo, _ColorShadow, 1.0 - lighting );
    
    return half4( color.rgb, main.a );
}


half4 FragmentProgram_Unlit( Varyings i ) : SV_Target
{
    float2 screenPos = i.positionNDC.xy;
    // Sample the required dither patterns.
    half ditherWorld64 = WorldDither64( i.positionWS.xy, 32 );

    half4 main = SampleMainTexture( i.uv );

    half3 albedo = CalculateAlbedo( main.r * main.g, main.b, ditherWorld64 );

    return half4( albedo.rgb, main.a );
}

#endif