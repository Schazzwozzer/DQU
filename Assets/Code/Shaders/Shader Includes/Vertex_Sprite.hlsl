#ifndef VERTEX_SPRITE_INCLUDED
#define VERTEX_SPRITE_INCLUDED

struct Attributes
{
    float3 positionOS   : POSITION;
    float2  uv          : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4  positionCS  : SV_POSITION;      // Position in homogeneous clip space
    float2	uv          : TEXCOORD0;
    float3  positionWS  : TEXCOORD2;        // Position in world space
    float4  positionNDC : TEXCOORD3;        // Position in homogeneous normalized device coordinates (screen space, generally)
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings VertexProgram_Sprite( Attributes i )
{
    Varyings o = (Varyings)0;
    UNITY_SETUP_INSTANCE_ID( i );
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

    o.uv = TRANSFORM_TEX( i.uv, _MainTex );

    VertexPositionInputs positions = GetVertexPositionInputs( i.positionOS );
    o.positionCS = positions.positionCS;// TransformObjectToHClip( v.positionOS );
    o.positionWS = positions.positionWS;//TransformObjectToWorld( v.positionOS );
    o.positionNDC = positions.positionNDC;

    return o;
}

#endif