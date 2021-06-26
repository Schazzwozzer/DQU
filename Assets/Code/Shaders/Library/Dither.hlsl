#ifndef DQU_DITHER_INCLUDED
#define DQU_DITHER_INCLUDED

#include "Assets/Code/Shaders/Library/Math.hlsl"

static float bayer2[2] = { 0.5 / 2, 1.5 / 2 };

static float bayer4[4] = {
    0.5 / 4,  2.5 / 4, 
    3.5 / 4,  1.5 / 4 };

static float bayer16[16] = {
     0.5 / 16,   8.5 / 16,   2.5 / 16,  10.5 / 16,
    12.5 / 16,   4.5 / 16,  14.5 / 16,   6.5 / 16,
     3.5 / 16,  11.5 / 16,   1.5 / 16,   9.5 / 16,
    15.5 / 16,   7.5 / 16,  13.5 / 16,   5.5 / 16 };

static float bayer64[64] = {
     0.5/64,  32.5/64,   8.5/64,  40.5/64,   2.5/64,  34.5/64,  10.5/64,  42.5/64,
    48.5/64,  16.5/64,  56.5/64,  24.5/64,  50.5/64,  18.5/64,  58.5/64,  26.5/64,
    12.5/64,  44.5/64,   4.5/64,  36.5/64,  14.5/64,  46.5/64,   6.5/64,  38.5/64,
    60.5/64,  28.5/64,  52.5/64,  20.5/64,  62.5/64,  30.5/64,  54.5/64,  22.5/64,
     3.5/64,  35.5/64,  11.5/64,  43.5/64,   1.5/64,  33.5/64,   9.5/64,  41.5/64,
    51.5/64,  19.5/64,  59.5/64,  27.5/64,  49.5/64,  17.5/64,  57.5/64,  25.5/64,
    15.5/64,  47.5/64,   7.5/64,  39.5/64,  13.5/64,  45.5/64,   5.5/64,  37.5/64,
    63.5/64,  31.5/64,  55.5/64,  23.5/64,  61.5/64,  29.5/64,  53.5/64,  21.5/64 };


/*****************************************************
/            Pre-calculated UV coordinates
*****************************************************/

float Dither4( float2 uv ) { return bayer4[int(uv.x) + int(uv.y) * 2]; }

float Dither16( float2 uv ) { return bayer16[int(uv.x) + int(uv.y) * 4]; }

float Dither64( float2 uv ) { return bayer64[int(uv.x) + int(uv.y) * 8]; }

/*****************************************************
/                   World Position
/ Calculate UV coordinates from world-space position.
/ 
*****************************************************/

float WorldDither4( float2 positionWS, int pixelsPerUnit )
{
    int2 uv = int2( mod( positionWS * pixelsPerUnit, 2.0) );
    return bayer4[uv.x + uv.y * 2];
}

float WorldDither16( float2 positionWS, int pixelsPerUnit )
{
    int2 uv = int2( mod( positionWS * pixelsPerUnit, 4.0) );
    return bayer16[uv.x + uv.y * 4];
}

float WorldDither64( float2 positionWS, int pixelsPerUnit )
{
    int2 uv = int2( mod( positionWS * pixelsPerUnit, 8.0 ) );
    return bayer64[uv.x + uv.y * 8];
}

/*****************************************************
/                   Screen Position
// Calculate UV coordinates from position on screen.
// Screen-space position is assumed to have already been normalized — divided by screenPos.w.
*****************************************************/

float ScreenDither2( float2 positionSS )
{
    positionSS *= _ScreenParams.xy;
    // This produces a checker pattern
    float dither = fmod( floor(positionSS.x) + floor(positionSS.y), 2.0 ) == 0.0;
    return 0.25 + dither * 0.5;

    int2 uv = int2( mod( positionSS * _ScreenParams.xy, 1 ) );
    return uv.x + uv.y;
    return bayer2[uv.x + uv.y];
}

float ScreenDither4( float2 positionSS )
{
    int2 uv = int2( mod( positionSS * _ScreenParams.xy, 2 ) );
    return bayer4[uv.x + uv.y * 2];
}

float ScreenDither16( float2 positionSS )
{
    int2 uv = int2( mod( positionSS * _ScreenParams.xy, 4 ) );
    return bayer16[uv.x + uv.y * 4];
}

float ScreenDither64( float2 positionSS )
{
    int2 uv = int2( mod( positionSS * _ScreenParams.xy, 8 ) );
    return bayer64[uv.x + uv.y * 8];
}

#endif