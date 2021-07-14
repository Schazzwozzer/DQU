using UnityEngine;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    public static class DQUProperties
    {
        public static readonly string BaseTextureMap = "_BaseMap";
        public static readonly int BaseTextureMapID = Shader.PropertyToID( BaseTextureMap );

        /// <summary>Used by Sprite shaders, as the default texture map.</summary>
        public static readonly string MainTextureMap = "_MainTex";
        public static readonly int MainTextureMapID = Shader.PropertyToID( MainTextureMap );

        public static readonly string ReceiveLighting = "_ReceiveLighting";
        public static readonly int ReceiveLightingID = Shader.PropertyToID( ReceiveLighting );

        public static readonly string ColorShadow = "_ColorShadow";
        public static readonly int ColorShadowID = Shader.PropertyToID( ColorShadow );

        public static readonly string Fresnel = "_Fresnel";
        public static readonly int FresnelID = Shader.PropertyToID( Fresnel );

        public static readonly string NoiseSettings = "_NoiseSettings";
        public static readonly int NoiseSettingsID = Shader.PropertyToID( NoiseSettings );

        // Support of paletted color

        public static readonly string HasColor2 = "_HasColor2";
        public static readonly int HasColor2ID  = Shader.PropertyToID( HasColor2 );

        public static readonly string HasColor3 = "_HasColor3";
        public static readonly int HasColor3ID  = Shader.PropertyToID( HasColor3 );

        public static readonly string HasColor4 = "_HasColor4";
        public static readonly int HasColor4ID  = Shader.PropertyToID( HasColor4 );

        public static readonly string Color1High = "_Color1High";
        public static readonly int Color1HighID  = Shader.PropertyToID( Color1High );
        public static readonly string Color1Low  = "_Color1Low";
        public static readonly int Color1LowID = Shader.PropertyToID( Color1Low );

        public static readonly string Color2High = "_Color2High";
        public static readonly int Color2HighID = Shader.PropertyToID( Color2High );
        public static readonly string Color2Low  = "_Color2Low";
        public static readonly int Color2LowID = Shader.PropertyToID( Color2Low );

        public static readonly string Color3High = "_Color3High";
        public static readonly int Color3HighID = Shader.PropertyToID( Color3High );
        public static readonly string Color3Low  = "_Color3Low";
        public static readonly int Color3LowID = Shader.PropertyToID( Color3Low );

        public static readonly string Color4High = "_Color4High";
        public static readonly int Color4HighID = Shader.PropertyToID( Color4High );
        public static readonly string Color4Low  = "_Color4Low";
        public static readonly int Color4LowID = Shader.PropertyToID( Color4Low );

        
    }
}
