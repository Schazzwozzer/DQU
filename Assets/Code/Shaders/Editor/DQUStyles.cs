using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    public static class DQUStyles
    {
        #region Copied from Unity's BaseShaderGUI.cs

        public static readonly GUIContent SurfaceOptions = new GUIContent( 
            "Surface Options", 
            "Controls how Universal RP renders the Material on a screen." );

        public static readonly GUIContent SurfaceInputs = new GUIContent( 
            "Surface Inputs",
            "These settings describe the look and feel of the surface itself." );

        public static readonly GUIContent AdvancedLabel = new GUIContent( 
            "Advanced",
            "These settings affect behind-the-scenes rendering and underlying calculations." );

        public static readonly GUIContent surfaceType = new GUIContent( 
            "Surface Type",
            "Select a surface type for your texture. Choose between Opaque or Transparent." );

        public static readonly GUIContent blendingMode = new GUIContent( 
            "Blending Mode",
            "Controls how the color of the Transparent surface blends with the Material color in the background." );

        public static readonly GUIContent cullingText = new GUIContent( 
            "Render Face",
            "Specifies which faces to cull from your geometry. Front culls front faces. Back culls backfaces. None means that both sides are rendered." );

        public static readonly GUIContent alphaClipText = new GUIContent( 
            "Alpha Clipping",
            "Makes your Material act like a Cutout shader. Use this to create a transparent effect with hard edges between opaque and transparent areas." );

        public static readonly GUIContent alphaClipThresholdText = new GUIContent( 
            "Threshold",
            "Sets where the Alpha Clipping starts. The higher the value is, the brighter the  effect is when clipping starts." );

        public static readonly GUIContent receiveShadowText = new GUIContent( 
            "Receive Shadows",
            "When enabled, other GameObjects can cast shadows onto this GameObject." );

        public static readonly GUIContent baseMap = new GUIContent( 
            "Base Map",
            "Specifies the base Material and/or Color of the surface. If you’ve selected Transparent or Alpha Clipping under Surface Options, your Material uses the Texture’s alpha channel or color." );

        public static readonly GUIContent emissionMap = new GUIContent( 
            "Emission Map",
            "Sets a Texture map to use for emission. You can also select a color with the color picker. Colors are multiplied over the Texture." );

        public static readonly GUIContent normalMapText = new GUIContent( 
            "Normal Map", 
            "Assigns a tangent-space normal map." );

        public static readonly GUIContent bumpScaleNotSupported = new GUIContent( "Bump scale is not supported on mobile platforms" );

        public static readonly GUIContent fixNormalNow = new GUIContent( 
            "Fix now",
            "Converts the assigned texture to be a normal map format." );

        public static readonly GUIContent queueSlider = new GUIContent( 
            "Priority",
            "Determines the chronological rendering order for a Material. High values are rendered first." );

        #endregion
    }
}
