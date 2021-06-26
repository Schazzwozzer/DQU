#ifndef DQU_SPRITE_PALETTE_FRAGMENT_INCLUDED
#define DQU_SPRITE_PALETTE_FRAGMENT_INCLUDED

#include "Assets/Code/Shaders/Library/BlendModes.hlsl"
#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/Math.hlsl"
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
    half ditherWorld16 = WorldDither16( i.positionWS.xy, 32 );
    half ditherWorld64 = WorldDither64( i.positionWS.xy, 32 );

    half4 main = SampleMainTexture( i.uv );

    // Start with the base color of the material.
    half3 albedo = CalculateAlbedo( main.r, main.b, ditherWorld64 );

#ifdef _RECEIVE_LIGHTING
    // Next up we'll incorporate lighting. Start by 
    // running the standard Unity lighting calculations.
    half lighting = CalculateShapeLightShared( screenPos );

    // Doing a few things here. First, we take the outer edge of the lighting, and blend it
    // with the texture's 'detail' map. Using this particular Blend function gives the shadows 
    // a nice, inky quality. Then we turn dither it, resulting in a 1-bit black & white value.
    half litSurface = step( ditherWorld16, BlendLinearBurn(main.g, smoothstep( 0.0, 0.5, lighting )));
    // Do that again, for the more strongly lit pixels, and add it to the previous result.
    litSurface += step( ditherWorld64, BlendLinearBurn( main.g, remap01( 0.5, 1.0, lighting )));
    // Then halve it all, resulting in three potential values: 0, 0.5, and 1.
    litSurface *= 0.5h;

    lighting = litSurface;
#else
    half lighting = step( ditherWorld64, main.g );
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