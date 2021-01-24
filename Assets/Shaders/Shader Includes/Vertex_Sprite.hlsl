#ifndef VERTEX_SPRITE_INCLUDED
#define VERTEX_SPRITE_INCLUDED

struct Attributes
{
    float3 positionOS   : POSITION;
    //float4 color        : COLOR;
    float2  uv          : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4  positionCS  : SV_POSITION;
    //half4   color       : COLOR;
    float2	uv          : TEXCOORD0;
    half2	lightingUV  : TEXCOORD1;
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings VertexProgram_Sprite(Attributes v)
{
    Varyings o = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID(v);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    o.positionCS = TransformObjectToHClip(v.positionOS);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    // Calculate normalized screen position [0..1] for lightmap.
    float4 clipVertex = o.positionCS / o.positionCS.w;
    o.lightingUV = ComputeScreenPos(clipVertex).xy;
    //o.color = v.color;
    return o;
}

#endif