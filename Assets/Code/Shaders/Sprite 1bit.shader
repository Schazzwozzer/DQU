Shader "DQU/Sprite"
{
    Properties
    {
        [MainTexture] _MainTex("Base Map (RGB) / Alpha (A)", 2D) = "white" {}
        //_MaskTex("Mask", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Color1High("Bright Color 1", Color) = (0.8, 0.8, 0.8, 1.0)
        _Color1Low("Dark Color 1", Color) = (0.4, 0.4, 0.4, 1.0)
        _HasColor2("Color 2", Float) = 0.0
        _Color2High("Bright Color 2", Color) = (0.8, 0.8, 0.8, 1.0)
        _Color2Low("Dark Color 2", Color) = (0.4, 0.4, 0.4, 1.0)
        _HasColor3("Color 3", Float) = 0.0
        _Color3High("Bright Color 3", Color) = (0.8, 0.8, 0.8, 1.0)
        _Color3Low("Dark Color 3", Color) = (0.4, 0.4, 0.4, 1.0)
        _HasColor4("Color 4", Float) = 0.0
        _Color4High("Bright Color 4", Color) = (0.8, 0.8, 0.8, 1.0)
        _Color4Low("Dark Color 4", Color) = (0.4, 0.4, 0.4, 1.0)
        _ColorStep("Step", Float) = 0.5

        _ColorShadow("Shadow Color", Color) = (0.27, 0.27, 0.36, 1.0)

        _NoiseSize("Noise Size", Float) = 1.0
        _NoiseOffset("Noise Offset", Vector) = (0, 0, 0, 0)

        [ToggleUI] _ReceiveLighting("Receive Lighting", Float) = 1.0

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

            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram_Lit
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

            #pragma shader_feature_local_fragment _ _RECEIVE_LIGHTING
            #pragma shader_feature_local_fragment _ _PALETTE_COUNT_2 _PALETTE_COUNT_3 _PALETTE_COUNT_4
            /*
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;
            half3 _ColorHigh, _ColorLow;
            half3 _ColorShadow = half3( 0.18, 0.2, 0.28 );
            */
            #include "Assets/Code/Shaders/Shader Includes/Sprite_Palette_Input.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Sprite_Palette_Vertex.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Sprite_Palette_Fragment.hlsl"

            ENDHLSL
        }
        /*
        Pass
        {
            Tags
            {
                "LightMode" = "UniversalForward"
                "Queue"="Transparent"
                "RenderType"="Transparent"
            }

            HLSLPROGRAM

            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram_Unlit
            /*
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;
            */
            #include "Assets/Code/Shaders/Shader Includes/Sprite_Palette_Input.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Sprite_Palette_Vertex.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Sprite_Palette_Fragment.hlsl"

            ENDHLSL
        }
        */
    }
    Fallback "Sprites/Default"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.SpritePaletteShaderGUI"
}