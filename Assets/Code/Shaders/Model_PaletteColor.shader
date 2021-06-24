Shader "DQU/Model"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (RGB) / Alpha (A)", 2D) = "white" {}
        [MainColor]   _BaseColor("Base Color", Color) = (1, 1, 1, 1)

        _Cutoff("Alpha Clipping", Range(0.0, 1.0)) = 0.5

        //_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
        //_SpecGlossMap("Specular Map", 2D) = "white" {}
        //[Enum(Specular Alpha,0,Albedo Alpha,1)] _SmoothnessSource("Smoothness Source", Float) = 0.0
        //[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0

        // DQU properties
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

        // Fresnel bias, scale, and power, and step value in one property.
        _Fresnel("Fresnel", Vector) = (0.01, 1.0, 1.0, 0.5)

        [ToggleUI] _ReceiveLighting("Receive Lighting", Float) = 1.0

        // Blending state
        [HideInInspector] _Surface("__surface", Float) = 0.0
        [HideInInspector] _Blend("__blend", Float) = 0.0
        [HideInInspector] _AlphaClip("__clip", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
        [HideInInspector] _Cull("__cull", Float) = 2.0

        //[ToggleOff] _ReceiveShadows("Receive Shadows", Float) = 1.0

        // Editmode props
        [HideInInspector] _QueueOffset("Queue offset", Float) = 0.0
        //[HideInInspector] _Smoothness("Smoothness", Float) = 0.5

        // ObsoleteProperties
        [HideInInspector] _MainTex("BaseMap", 2D) = "white" {}
        [HideInInspector] _Color("Base Color", Color) = (1, 1, 1, 1)
        //[HideInInspector] _Shininess("Smoothness", Float) = 0.0
        //[HideInInspector] _GlossinessSource("GlossinessSource", Float) = 0.0
        //[HideInInspector] _SpecSource("SpecularHighlights", Float) = 0.0
    }

    HLSLINCLUDE

    ENDHLSL

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            //"UniversalMaterialType" = "SimpleLit"
            "IgnoreProjector" = "True"
            "ShaderModel"="4.5"
        }
        LOD 300

        Pass
        {
            Name "Universal2D"
            Tags
            {
                "LightMode" = "Universal2D"
                "RenderType" = "Transparent"
                "Queue" = "Transparent"
            }

            HLSLPROGRAM
            #pragma only_renderers gles gles3 glcore d3d11
            #pragma target 2.0

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
            //#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
            //#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
            //#pragma shader_feature_local _NORMALMAP
            //#pragma shader_feature_local_fragment _EMISSION
            //#pragma shader_feature_local _RECEIVE_SHADOWS_OFF

            #pragma shader_feature_local_fragment _ _RECEIVE_LIGHTING
            #pragma shader_feature_local_fragment _ _PALETTE_COUNT_2 _PALETTE_COUNT_3 _PALETTE_COUNT_4

            //--------------------------------------
            // GPU Instancing
            #pragma multi_compile_instancing
            #pragma multi_compile _ DOTS_INSTANCING_ON
            // Support 2D lighting
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

            #pragma vertex VertexProgram
            #pragma fragment FragmentProgram

            #include "Assets/Code/Shaders/Shader Includes/Model_Palette_Input.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Model_Vertex.hlsl"
            #include "Assets/Code/Shaders/Shader Includes/Model_Palette_Fragment.hlsl"
            ENDHLSL
        }
    }
    Fallback "Hidden/Universal Render Pipeline/FallbackError"
    CustomEditor "UnityEditor.Rendering.Universal.ShaderGUI.ModelShaderGUI"
}