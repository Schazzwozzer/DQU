#ifndef NOISE_HELPERS_INCLUDED
#define NOISE_HELPERS_INCLUDED

#include "NoiseLib.hlsl"

half FractalPerlin2D( int octaves, float2 pos, half scale )
{
    half value = 0.5h,
         w = 0.5h;
    for( int i = 0; i < octaves; ++i )
    {
        half noise = Perlin2D( pos * scale );
        value += noise * w;
        scale *= 2;		// Lacunarity
        w *= 0.5;		// Gain
    }
    return value;
}

#endif