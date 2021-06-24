#ifndef DQU_SPRITE_ENVIRONMENT_FRAGMENT_INCLUDED
#define DQU_SPRITE_ENVIRONMENT_FRAGMENT_INCLUDED

#include "Assets/Code/Shaders/Library/BlendModes.hlsl"
#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/NoiseHelpers.hlsl"
#include "Assets/Code/Shaders/Library/PaletteColors.hlsl"
#include "Assets/Code/Shaders/Library/Official Edits/Lighting.hlsl"


half4 SampleMainTexture( float2 uv )
{
    // Sample the main albedo/diffuse texture.
    // Red channel attenuates noise-based color blending.
    // Green channel is detail/shadow map.
    // Blue channel are color/tone masks.
    return SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, uv, 0);
}


// Determine the material's base color.
half3 CalculateAlbedo( half colorBlend, half colorMask, half ditherPattern, float3 positionWS )
{
    float2 noiseOffset = _NoiseSettings.xy;
    float noiseScale = _NoiseSettings.z;

    // Calculate a procedural perlin noise, which we will use for some subtle visual interest.
    half noise = FractalPerlin2D( 4, positionWS.xy + noiseOffset, noiseScale );

    // Factor the noise into the color blend and then convert to a 1-bit value.
    colorBlend = step( BlendOverlay( colorBlend, noise ), ditherPattern );

    // Determine our two colors — the high tone and low tone.
    half3 lowColor, highColor;
    GetPaletteColors( colorMask, lowColor, highColor );

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
    half3 albedo = CalculateAlbedo( main.r, main.b, ditherWorld64, i.positionWS );

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

    half3 albedo = CalculateAlbedo( main.r * main.g, main.b, ditherWorld64, i.positionWS );

    return half4( albedo.rgb, main.a );
}

#endif