#ifndef DQU_MODEL_1BIT_FRAGMENT_NCLUDED
#define DQU_MODEL_1BIT_FRAGMENT_NCLUDED

#include "Assets/Code/Shaders/Library/DQUCommon.hlsl"
#include "Assets/Code/Shaders/Library/BlendModes.hlsl"
#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/NoiseHelpers.hlsl"
#include "Assets/Code/Shaders/Library/PaletteColors.hlsl"
#include "Assets/Code/Shaders/Library/Official Edits/Lighting.hlsl"

uniform float _NoiseSize;
uniform float2 _NoiseOffset;

half4 FragmentProgram_Model_1bit( Varyings i ) : SV_Target
{
    // Sample the main albedo/diffuse texture.
    // Red channel is currently not used for this shader.
    // Green channel is detail/shadow map.
    // Blue channel are color/tone masks.
    half4 main = SAMPLE_TEXTURE2D_LOD(_BaseMap, sampler_BaseMap, i.uv, 0);
    AlphaDiscard( main.a, _Cutoff );

    float2 screenPos = i.positionNDC.xy;
    float3 normalWS = normalize( i.normalWS );

    /**** BASE ALBEDO COLOR ****/

    // Determine our two colors — the high tone and low tone.
    half3 lowColor, highColor;
    GetPaletteColors( main.b, lowColor, highColor );

    // Since we're trying to roughly emulate pixel art, 
    // let's start with an outline, using fresnel.
    half outline = CalculateFresnel( _Fresnel.x, _Fresnel.y, _Fresnel.z, i.viewDirWS, normalWS );
    outline = step( 1.0 - _Fresnel.w, outline );    // Step it to either get 0 or 1.

    // Then we just subtract the outline from the texture's detail map (which is also stepped).
    half detail = max( 0, step( _ColorStep, main.g ) - outline );

    half3 albedo = lerp( lowColor, highColor, detail );     // The actual albedo color.

    /**** LIGHTING ****/

#ifdef _RECEIVE_LIGHTING
    // Run the standard Unity lighting calculations.
    half lighting = CalculateShapeLightShared( screenPos ); // 0 is full shadow, 1 is fully lit

    // The 2D 'shape' lighting doesn't wrap around the model at all, so let's do a
    // simple NdotL, using an arbitrary "from up above" vector as the light direction.
    half fauxLighting = saturate( dot( normalize( float3( 0, 0.75, -1)), normalWS ) * 0.5 + 0.5 );

    // By using this blend function, the NdotL shadows will get
    // stronger as the model moves out from 2D lit areas, until fully shaded.
    // Notice also that the 2D lighting value is squared; I feel this produces better results.
    lighting = BlendColorBurn( fauxLighting, max( 0, lighting * lighting ) );

    // Finally, split it into three tones: full shadow, 50% shadow, and fully lit.
    lighting = step( 0.5, lighting ) * 0.5 + 
               step( 0.1, lighting ) * 0.5;
#else
    half lighting = 1.0;
#endif

    // Combine albedo with lighting/shadows for final color.
    half3 color = lerp( albedo, _ColorShadow, 1.0 - lighting );
    
    return half4( color.rgb, main.a );
}

#endif