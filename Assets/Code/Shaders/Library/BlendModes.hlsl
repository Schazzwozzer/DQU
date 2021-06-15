/*
** Copyright (c) 2012, Romain Dura romain@shazbits.com
** 
** Permission to use, copy, modify, and/or distribute this software for any 
** purpose with or without fee is hereby granted, provided that the above 
** copyright notice and this permission notice appear in all copies.
** 
** THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES 
** WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF 
** MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY 
** SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES 
** WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN 
** ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF OR 
** IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
*/

/*
** Photoshop & misc math
** Blending modes, RGB/HSL/Contrast/Desaturate, levels control
**
** Romain Dura | Romz
** Blog: http://mouaif.wordpress.com
** Post: http://mouaif.wordpress.com/?p=94
*/

// Romain Dura's original code heavily used #define directives, 
// which Unity disliked. I've adapted it to functions, with 
// corresponding versions that operate with half-precision values.

#ifndef BLEND_MODES_INCLUDED
#define BLEND_MODES_INCLUDED

/***************************/
/*     Lighten & Darken    */
/***************************/
half BlendDarken( half base, half blend ) { return min(blend, base); }
float BlendDarken( float base, float blend ) { return min(blend, base); }

half BlendLighten( half base, half blend ) { return max(blend, base); }
float BlendLighten( float base, float blend ) { return max(blend, base); }

/***************************/
/*    Color Dodge / Burn   */
/***************************/
half BlendColorBurn( half base, half blend )
{
    return ((blend == 0.0h) * blend +
        (blend != 0.0h) * max((1.0h - ((1.0h - base) / blend)), 0.0h));
}
float BlendColorBurn( float base, float blend )
{
    return ((blend == 0.0) * blend +
        (blend != 0.0) * max((1.0 - ((1.0 - base) / blend)), 0.0));
}
half3 BlendColorBurn( half3 base, half3 blend )
{
    return half3( BlendColorBurn( base.r, blend.r ),
                  BlendColorBurn( base.g, blend.g ),
                  BlendColorBurn( base.b, blend.b ) );
}
float3 BlendColorBurn( float3 base, float3 blend )
{
    return float3( BlendColorBurn( base.r, blend.r ),
                   BlendColorBurn( base.g, blend.g ),
                   BlendColorBurn( base.b, blend.b ) );
}

half BlendColorDodge( half base, half blend )
{
    return ((blend == 1.0h) * blend + 
        (blend != 1.0h) * min(base / (1.0h - blend), 1.0h));
}
float BlendColorDodge( float base, float blend )
{
    return ((blend == 1.0) * blend + 
        (blend != 1.0) * min(base / (1.0 - blend), 1.0));
}
half3 BlendColorDodge( half3 base, half3 blend )
{
    return ((blend == 1.0h) * blend + 
        (blend != 1.0h) * min(base / (1.0h - blend), 1.0h));
}
float3 BlendColorDodge( float3 base, float3 blend )
{
    return ((blend == 1.0) * blend + 
        (blend != 1.0) * min(base / (1.0 - blend), 1.0));
}

/***************************/
/*   Linear Dodge / Burn   */
/***************************/
half BlendLinearDodge( half base, half blend ) { return min(base + blend, 1.0h); }
float BlendLinearDodge( float base, float blend ) { return min(base + blend, 1.0); }
half3 BlendLinearDodge( half3 base, half3 blend ) { return min(base + blend, 1.0h); }
float3 BlendLinearDodge( float3 base, float3 blend ) { return min(base + blend, 1.0); }

half BlendLinearBurn( half base, half blend ) { return max(base + blend - 1.0h, 0.0h); }
float BlendLinearBurn( float base, float blend ) { return max(base + blend - 1.0, 0.0); }
half3 BlendLinearBurn( half3 base, half3 blend ) { return max(base + blend - 1.0h, 0.0h); }
float3 BlendLinearBurn( float3 base, float3 blend ) { return max(base + blend - 1.0, 0.0); }

/***************************/
/*         Screen          */
/***************************/

half BlendScreen( half base, half blend ) { return 1.0h - (1.0h - base) * (1.0h - blend); }
float BlendScreen( float base, float blend ) { return 1.0 - (1.0 - base) * (1.0 - blend); }

half3 BlendScreen( half3 base, half3 blend ) { return 1.0h - (1.0h - base) * (1.0h - blend); }
float3 BlendScreen( float3 base, float3 blend ) { return 1.0 - (1.0 - base) * (1.0 - blend); }

/********************/
/*      Overlay     */
/********************/
half BlendOverlay( half base, half blend )
{
    return 
        ((base < 0.5h) * (2.0h * base * blend) +
        (base >= 0.5h) * (1.0h - 2.0h * (1.0h - base) * (1.0h - blend)));
}

half3 BlendOverlay( half3 base, half3 blend )
{
    return half3( BlendOverlay( base.r, blend.r ),
                  BlendOverlay( base.g, blend.g ),
                  BlendOverlay( base.b, blend.b ) );
}

float BlendOverlay( float base, float blend )
{
    return 
        ((base < 0.5) * (2.0 * base * blend) +
        (base >= 0.5) * (1.0 - 2.0 * (1.0 - base) * (1.0 - blend)));
}

float3 BlendOverlay( float3 base, float3 blend )
{
    return float3( BlendOverlay( base.r, blend.r ),
                   BlendOverlay( base.g, blend.g ),
                   BlendOverlay( base.b, blend.b ) );
}

/********************/
/*    Soft light    */
/********************/
half BlendSoftLight( half base, half blend )
{
    return 
        ((blend < 0.5h) * (2.0h * base * blend + base * base * (1.0h - 2.0h * blend)) + 
        (blend >= 0.5h) * (sqrt(base) * (2.0h * blend - 1.0h) + 2.0h * base * (1.0h - blend)));
}

half3 BlendSoftLight( half3 base, half3 blend )
{
    return half3( BlendSoftLight( base.r, blend.r ),
                  BlendSoftLight( base.g, blend.g ),
                  BlendSoftLight( base.b, blend.b ) );
}

float BlendSoftLight( float base, float blend )
{
    return 
        ((blend < 0.5) * (2.0 * base * blend + base * base * (1.0 - 2.0 * blend)) + 
        (blend >= 0.5) * (sqrt(base) * (2.0 * blend - 1.0) + 2.0 * base * (1.0 - blend)));
}

float3 BlendSoftLight( float3 base, float3 blend )
{
    return float3( BlendSoftLight( base.r, blend.r ),
                   BlendSoftLight( base.g, blend.g ),
                   BlendSoftLight( base.b, blend.b ) );
}

/********************/
/*    Hard light    */
/********************/
half BlendHardLight( half base, half blend )
{
    return BlendOverlay( blend, base );
}

half3 BlendHardLight( half3 base, half3 blend )
{
    return half3( BlendHardLight( base.r, blend.r ),
                  BlendHardLight( base.g, blend.g ),
                  BlendHardLight( base.b, blend.b ) );
}

float BlendHardLight( float base, float blend )
{
    return BlendOverlay( blend, base );
}

float3 BlendHardLight( float3 base, float3 blend )
{
    return float3( BlendHardLight( base.r, blend.r ),
                   BlendHardLight( base.g, blend.g ),
                   BlendHardLight( base.b, blend.b ) );
}

/********************/
/*    Vivid light   */
/********************/
half BlendVividLight( half base, half blend )
{
    return 
        ((blend < 0.5h) * BlendColorBurn(base, (2.0h * blend)) +
        (blend >= 0.5h) * BlendColorDodge(base, (2.0h * (blend - 0.5h))));
}

half3 BlendVividLight( half3 base, half3 blend )
{
    return half3( BlendVividLight( base.r, blend.r ),
                  BlendVividLight( base.g, blend.g ),
                  BlendVividLight( base.b, blend.b ) );
}

float BlendVividLight( float base, float blend )
{
    return 
        ((blend < 0.5) * BlendColorBurn(base, (2.0 * blend)) +
        (blend >= 0.5) * BlendColorDodge(base, (2.0 * (blend - 0.5))));
}

float3 BlendVividLight( float3 base, float3 blend )
{
    return float3( BlendVividLight( base.r, blend.r ),
                   BlendVividLight( base.g, blend.g ),
                   BlendVividLight( base.b, blend.b ) );
}

/********************/
/*   Linear light   */
/********************/

half BlendLinearLight( half base, half blend )
{
    return 
        ((blend < 0.5h) * BlendLinearBurn(base, (2.0h * blend)) + 
        (blend >= 0.5h) * BlendLinearDodge(base, (2.0h * (blend - 0.5h))));
}

half3 BlendLinearLight( half3 base, half3 blend )
{
    return half3( BlendLinearLight( base.r, blend.r ),
                  BlendLinearLight( base.g, blend.g ),
                  BlendLinearLight( base.b, blend.b ) );
}

float BlendLinearLight( float base, float blend )
{
    return 
        ((blend < 0.5) * BlendLinearBurn(base, (2.0 * blend)) + 
        (blend >= 0.5) * BlendLinearDodge(base, (2.0 * (blend - 0.5))));
}

float3 BlendLinearLight( float3 base, float3 blend )
{
    return float3( BlendLinearLight( base.r, blend.r ),
                   BlendLinearLight( base.g, blend.g ),
                   BlendLinearLight( base.b, blend.b ) );
}

/********************/
/*     Pin light    */
/********************/
half BlendPinLight( half base, half blend )
{
    return 
        ((blend < 0.5h) * BlendDarken(base, (2.0h * blend)) +
        (blend >= 0.5h) * BlendLighten(base, (2.0h * (blend - 0.5h))));
}

half3 BlendPinLight( half3 base, half3 blend )
{
    return half3( BlendPinLight( base.r, blend.r ),
                  BlendPinLight( base.g, blend.g ),
                  BlendPinLight( base.b, blend.b ) );
}

float BlendPinLight( float base, float blend )
{
    return 
        ((blend < 0.5) * BlendDarken(base, (2.0 * blend)) +
        (blend >= 0.5) * BlendLighten(base, (2.0 * (blend - 0.5))));
}

float3 BlendPinLight( float3 base, float3 blend )
{
    return float3( BlendPinLight( base.r, blend.r ),
                   BlendPinLight( base.g, blend.g ),
                   BlendPinLight( base.b, blend.b ) );
}

#endif