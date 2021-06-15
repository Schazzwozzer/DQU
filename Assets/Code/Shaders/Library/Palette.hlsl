#ifndef DQU_PALETTE_INCLUDED
#define DQU_PALETTE_INCLUDED

void GetPaletteColors( 
    float paletteMaskValue, 
    out half3 lowColor, out half3 highColor )
{
#ifdef _PALETTE_COUNT_4
    half paletteCount = 4;
#elif _PALETTE_COUNT_3
    half paletteCount = 3;
#elif _PALETTE_COUNT_2
    half paletteCount = 2;
#else
    half paletteCount = 1;
#endif

    half paletteNumber = floor( paletteMaskValue * paletteCount );

    lowColor = 
        (paletteNumber == 0) * _Color1Low
    #if defined(_PALETTE_COUNT_4) || defined(_PALETTE_COUNT_3) || defined(_PALETTE_COUNT_2)
        + (paletteNumber == 1) * _Color2Low
    #endif
    #if defined(_PALETTE_COUNT_4) || defined(_PALETTE_COUNT_3)
        + (paletteNumber == 2) * _Color3Low
    #endif
    #if defined(_PALETTE_COUNT_4)
        + (paletteNumber == 3) * _Color4Low
    #endif
    ;

    highColor = 
        (paletteNumber == 0) * _Color1High
    #if defined(_PALETTE_COUNT_4) || defined(_PALETTE_COUNT_3) || defined(_PALETTE_COUNT_2)
        + (paletteNumber == 1) * _Color2High
    #endif
    #if defined(_PALETTE_COUNT_4) || defined(_PALETTE_COUNT_3)
        + (paletteNumber == 2) * _Color3High
    #endif
    #if defined(_PALETTE_COUNT_4)
        + (paletteNumber == 3) * _Color4High
    #endif
    ;
}

#endif