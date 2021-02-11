#ifndef FRAGMENT_SPRITE_INCLUDED
#define FRAGMENT_SPRITE_INCLUDED

#include "Assets/Code/Shaders/Library/Dither.hlsl"
#include "Assets/Code/Shaders/Library/Official Edits/Lighting.hlsl"

half4 FragmentProgram_Sprite( Varyings i ) : SV_Target
{
    // Sample the main albedo/diffuse texture.
    half4 main = SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex, i.uv, 0);

    // Calculate lighting.
    half lighting = CalculateShapeLightShared( i.lightingUV );

    half dither64 = ScreenDither64( i.lightingUV );

    // Combine the sampled albedo and the lighting, and dither the result.
    half albedo = step( dither64, main.r * lighting );

    half3 color = lerp( _ColorLow, _ColorHigh, albedo );
    //return half4( color.rgb, 1.0 );

    //lighting = ( step( lighting, 0.3333333 ) + 
    //             step( lighting, 0.6666667 ) ) / 2;

    //lighting = (smoothstep( 0.15, 0.35, 1.0 - lighting ) + 
    //            smoothstep( 0.65, 0.85, 1.0 - lighting )) / 2;

    //lighting = pow( lighting, 2.2 );
    //lighting *= albedo;
    lighting = step( ScreenDither64( i.lightingUV ), lighting );

    color = lerp( color, _ColorShadow, 1.0 - lighting );
    return half4( color.rgb, 1.0 );

    return half4( lighting, lighting, lighting, 1.0 );
    
    return half4( albedo, albedo, albedo, 1.0 );

}

#endif