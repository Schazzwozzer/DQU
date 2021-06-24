#ifndef DQU_MODEL_1BIT_VERTEX_INCLUDED
#define DQU_MODEL_1BIT_VERTEX_INCLUDED

// Access to GetViewForwardDir().
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

struct Attributes
{
    float3 positionOS   : POSITION;         // Position in object space
    float2  uv          : TEXCOORD0;
    float3 normalOS     : NORMAL;           // Surface normal in object space
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings
{
    float4  positionCS  : SV_POSITION;      // Position in homogeneous clip space
    float2	uv          : TEXCOORD0;
    float3  positionWS  : TEXCOORD1;        // Position in world space
    float4  positionNDC : TEXCOORD2;        // Position in homogeneous normalized device coordinates (screen space, generally)
    float3  normalWS    : TEXCOORD3;        // Surface normal in world space
    float3  viewDirWS   : TEXCOORD4;        // Camera direction in world space
    UNITY_VERTEX_INPUT_INSTANCE_ID
    UNITY_VERTEX_OUTPUT_STEREO
};

Varyings VertexProgram_Model_1bit( Attributes i )
{
    Varyings o = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID( i );
    UNITY_TRANSFER_INSTANCE_ID( i, o );
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

    o.uv = TRANSFORM_TEX( i.uv, _BaseMap );

    VertexPositionInputs positions = GetVertexPositionInputs( i.positionOS );
    o.positionCS = positions.positionCS;
    o.positionWS = positions.positionWS;
    o.positionNDC = positions.positionNDC;

    o.normalWS = TransformObjectToWorldNormal( i.normalOS );
    o.viewDirWS = GetWorldSpaceViewDir( o.positionWS );

    return o;
}

#endif