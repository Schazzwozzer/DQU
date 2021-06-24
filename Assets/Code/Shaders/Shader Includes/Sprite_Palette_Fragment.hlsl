#ifndef FRAGMENT_SPRITE_INCLUDED
#define FRAGMENT_SPRITE_INCLUDED

#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/NoiseHelpers.hlsl"
#include "Assets/Code/Shaders/Library/PaletteColors.hlsl"
#include "Assets/Code/Shaders/Library/Official Edits/Lighting.hlsl"

uniform float _NoiseSize;
uniform float2 _NoiseOffset;


half4 SampleMainTexture( float2 uv )
{
    // Sample the main albedo/diffuse texture.
    // Red channel attenuates noise-based color blending.
    // Green channel is detail/shadow map.
    // Blue channel are color/tone masks.
    return SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, uv, 0);
}


// Determine the material's base color.
half3 CalculateAlbedo( float3 positionWS, half colorMask, half ditherPattern )
{
    // Calculate a procedural perlin noise, which we will use for some subtle visual interest.
    half noise = FractalPerlin2D( 4, positionWS.xy + _NoiseOffset, _NoiseSize );
    noise = step( ditherPattern, noise );

    // Determine our two colors — the high tone and low tone.
    half3 lowColor, highColor;
    GetPaletteColors( colorMask, lowColor, highColor );

    return lerp( lowColor, highColor, noise );
}



half4 FragmentProgram_Lit( Varyings i ) : SV_Target
{
    float2 screenPos = i.positionNDC.xy;
    // Sample the required dither patterns.
    half ditherScreen64 = ScreenDither64( screenPos );
    //half ditherScreen16 = ScreenDither16( screenPos );
    half ditherScreen4 = ScreenDither4( screenPos );
    //half ditherScreen2 = ScreenDither2( screenPos );
    half ditherWorld64 = WorldDither64( i.positionWS.xy, 32 );

    half4 main = SampleMainTexture( i.uv );

    // Determine our two colors — the high tone and low tone.
    half3 lowColor, highColor;
    GetPaletteColors( main.b, lowColor, highColor );

    // Start with the base color of the material.
    half3 albedo = CalculateAlbedo( i.positionWS, main.b, ditherWorld64 );

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

    half3 albedo = CalculateAlbedo( i.positionWS, main.b, ditherWorld64 );

    return half4( albedo.rgb, main.a );
}

#endif