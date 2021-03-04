Shader "DQU/Sprite"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}
        //_MaskTex("Mask", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _ColorHigh("Bright Color", Color) = (0.8, 0.8, 0.8, 1.0)
        _ColorLow("Dark Color", Color) = (0.4, 0.4, 0.4, 1.0)
        _ColorShadow("Shadow Color", Color) = (0.15, 0.15, 0.3, 1.0)

        _NoiseSize("Noise Size", Float) = 1.0
        _NoiseOffset("Noise Offset", Vector) = (0, 0, 0, 0)

        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    HLSLINCLUDE
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    ENDHLSL

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }
            HLSLPROGRAM

            #pragma vertex VertexProgram_Sprite
            #pragma fragment FragmentProgram_Sprite
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;
            half3 _ColorHigh, _ColorLow;
            half3 _ColorShadow = half3( 0.18, 0.2, 0.28 );

            #include "Assets/Code/Shaders/Shader Includes/Vertex_Sprite.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Fragment_Sprite.hlsl"

            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}