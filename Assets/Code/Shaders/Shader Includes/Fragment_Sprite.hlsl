#ifndef FRAGMENT_SPRITE_INCLUDED
#define FRAGMENT_SPRITE_INCLUDED

#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/NoiseHelpers.hlsl"
#include "Assets/Code/Shaders/Library/Official Edits/Lighting.hlsl"

uniform float _NoiseSize;
uniform float2 _NoiseOffset;

half4 FragmentProgram_Sprite( Varyings i ) : SV_Target
{
    // Sample the main albedo/diffuse texture.
    // Red channel attenuates noise-based color blending.
    // Green channel is detail/shadow map.
    // Blue channel are color/tone masks.
    half4 main = SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, i.uv, 0);

    float2 screenPos = i.positionNDC.xy;
    // Sample the required dither patterns.
    half ditherScreen64 = ScreenDither64( screenPos );
    //half ditherScreen16 = ScreenDither16( screenPos );
    half ditherScreen4 = ScreenDither4( screenPos );
    //half ditherScreen2 = ScreenDither2( screenPos );
    half ditherWorld64 = WorldDither64( i.positionWS.xy, 32 );

    // First let's calculate the base color of the material, which we'll just call 'albedo'.

    // Calculate a procedural perlin noise, which we will use for some subtle visual interest.
    half noise = FractalPerlin2D( 4, i.positionWS.xy + _NoiseOffset, _NoiseSize );
    noise = step( ditherWorld64, noise );

    half3 albedo = lerp( _ColorLow, _ColorHigh, noise );

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

    // Combine albedo with lighting/shadows for final color.
    half3 color = lerp( albedo, _ColorShadow, 1.0 - lighting );
    
    return half4( color.rgb, main.a );
}

#endif