using System;
using UnityEngine;
using UnityEngine.Rendering;
using DQU.Editor;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class SpriteEnvironmentShaderGUI : SpriteBaseShaderGUI
    {
        protected LightingFeature _lightingFeature = new LightingFeature();
        protected PaletteFeature _paletteFeature = new PaletteFeature();

        protected MaterialProperty p_noiseSettings;

        protected GUIContent l_noiseScale = new GUIContent( "Noise Scale" );
        protected GUIContent l_noiseOffset = new GUIContent( "Noise Offset" );


        public override void FindProperties( MaterialProperty[] properties )
        {
            base.FindProperties( properties );

            _lightingFeature.FindProperties( properties );
            _paletteFeature.FindProperties( properties );

            p_noiseSettings = FindProperty( DQUProperties.NoiseSettings, properties );
        }


        public override void MaterialChanged( Material material )
        {
            if( material == null )
                throw new ArgumentNullException( "material" );

            SetMaterialKeywords( material );
        }

        protected void SetMaterialKeywords( Material material )
        {
            BaseShaderGUI.SetMaterialKeywords( material );

            _lightingFeature.SetMaterialKeywords( material );
            _paletteFeature.SetMaterialKeywords( material );
        }


        // material main surface options
        public override void DrawSurfaceOptions( Material material )
        {
            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            base.DrawSurfaceOptions( material );

            _lightingFeature.DrawSurfaceOptions( material );
        }

        // material main surface inputs
        public override void DrawSurfaceInputs( Material material )
        {
            base.DrawSurfaceInputs( material );

            _paletteFeature.DrawSurfaceInputs( material );

            DrawNoiseSettings();
        }


        protected void DrawNoiseSettings()
        {
            EditorGUI.BeginChangeCheck();

            Vector2 offset = EditorGUILayout.Vector2Field( l_noiseOffset, p_noiseSettings.vectorValue );
            float scale = EditorGUILayout.FloatField( l_noiseScale, 1f / p_noiseSettings.vectorValue.z );

            if( EditorGUI.EndChangeCheck() )
                p_noiseSettings.vectorValue = new Vector4( offset.x, offset.y, 1f / scale, 0 );
        }


    }
}
