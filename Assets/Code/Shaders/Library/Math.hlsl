#ifndef DQU_MATH_INCLUDED
#define DQU_MATH_INCLUDED

// The modulus functions of HLSL and GLSL differ slightly,
// specifically in how the return value's sign is determined.
// Ben Golus spells it out nicely here: 
// https://forum.unity.com/threads/translating-a-glsl-shader-noise-algorithm-to-hlsl-cg.485750/#post-3164874
//
// Anyhow, this is the GLSL modulus function.
#define mod(x, y) (x - y * floor(x / y))


// https://en.wikipedia.org/wiki/Smoothstep
float smootherstep( float edge0, float edge1, float x )
{
    x = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
    return x * x * x * (x * (x * 6.0 - 15.0) + 10.0);
}

// Also from https://en.wikipedia.org/wiki/Smoothstep
float inverseSmoothstep(float x)
{
    return 0.5 - sin(asin(1.0 - 2.0 * x) / 3.0);
}


// Maps 'value' from an original numerical range to a range of [0..1], kind of
// like smoothstep, but without the smoothing. Also does not clamp output value.
// originalMin is the lowest value in the original range.
// originalMin is the highest value in the original range.
// value is the value to be remapped/scaled.
float remap01( float originalMin, float originalMax, float value )
{
    return ( value - originalMin ) / ( originalMax - originalMin );
}


// Maps 'value' from one numerical range to another.
// originalMin is the lowest value in the original range.
// originalMax is the highest value in the original range.
// value is the value to be remapped/scaled.
// newMin is the lowest value in the new target range.
// NewMax is the highest value in the new target range.
float remap( float originalMin, float originalMax, float value, float newMin, float newMax )
{
    return newMin + (newMax - newMin) * ((value - originalMin) / (originalMax - originalMin));
}

#endif