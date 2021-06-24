using System;
using UnityEngine;
using UnityEngine.Rendering;
using DQU.Editor;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class ModelShaderGUI : BaseShaderGUI
    {
        protected LightingFeature _lightingFeature = new LightingFeature();
        protected PaletteFeature _paletteFeature = new PaletteFeature();

        // Properties
        protected MaterialProperty p_fresnel;

        // GUI Content Labels
        protected GUIContent l_fresnel = new GUIContent( "Fresnel Outline" ),
                             l_bias    = new GUIContent( "Bias" ),
                             l_scale   = new GUIContent( "Scale" ),
                             l_power   = new GUIContent( "Power" ),
                             l_step    = new GUIContent( "Step Value" );


        public override void FindProperties( MaterialProperty[] properties )
        {
            base.FindProperties( properties );

            _lightingFeature.FindProperties( properties );
            _paletteFeature.FindProperties( properties );

            p_fresnel = FindProperty( DQUProperties.Fresnel, properties );
        }


        // material changed check
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
            if( material == null )
                throw new ArgumentNullException( "material" );

            // Use default labelWidth
            EditorGUIUtility.labelWidth = 0f;

            // Detect any changes to the material
            EditorGUI.BeginChangeCheck();
            {
                base.DrawSurfaceOptions( material );

                _lightingFeature.DrawSurfaceOptions( material );
            }
            if( EditorGUI.EndChangeCheck() )
            {
                foreach( var obj in blendModeProp.targets )
                    MaterialChanged( (Material)obj );
            }
        }

        // material main surface inputs
        public override void DrawSurfaceInputs( Material material )
        {
            base.DrawSurfaceInputs( material );

            _paletteFeature.DrawSurfaceInputs( material );

            DrawFresnelSettings( material );
        }



        protected void DrawFresnelSettings( Material material )
        {
            EditorGUILayout.LabelField( l_fresnel, CustomGUIStyles.LabelBold );

            int oldIndent = EditorGUI.indentLevel;
            ++EditorGUI.indentLevel;

            Vector4 newValue = Vector4.zero;

            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = p_fresnel.hasMixedValue;

            newValue.x = EditorGUILayout.Slider( l_bias, p_fresnel.vectorValue.x, 0f, 1f );         // Fresnel bias
            newValue.y = EditorGUILayout.Slider( l_scale, p_fresnel.vectorValue.y, 0f, 10f );       // Fresnel scale
            newValue.z = EditorGUILayout.Slider( l_power, p_fresnel.vectorValue.z, 0f, 10f );       // Fresnel power
            newValue.w = EditorGUILayout.Slider( l_step, p_fresnel.vectorValue.w, 0f, 1f );         // Outline step

            if( EditorGUI.EndChangeCheck() )
                p_fresnel.vectorValue = newValue;
                
            EditorGUI.showMixedValue = false;
            EditorGUI.indentLevel = oldIndent;
        }


        public override void AssignNewShaderToMaterial( Material material, Shader oldShader, Shader newShader )
        {
            if( material == null )
                throw new ArgumentNullException( "material" );

            // _Emission property is lost after assigning Standard shader to the material
            // thus transfer it before assigning the new shader
            if( material.HasProperty( "_Emission" ) )
            {
                material.SetColor( "_EmissionColor", material.GetColor( "_Emission" ) );
            }

            base.AssignNewShaderToMaterial( material, oldShader, newShader );

            if( oldShader == null || !oldShader.name.Contains( "Legacy Shaders/" ) )
            {
                SetupMaterialBlendMode( material );
                return;
            }

            SurfaceType surfaceType = SurfaceType.Opaque;
            BlendMode blendMode = BlendMode.Alpha;
            if( oldShader.name.Contains( "/Transparent/Cutout/" ) )
            {
                surfaceType = SurfaceType.Opaque;
                material.SetFloat( "_AlphaClip", 1 );
            }
            else if( oldShader.name.Contains( "/Transparent/" ) )
            {
                // NOTE: legacy shaders did not provide physically based transparency
                // therefore Fade mode
                surfaceType = SurfaceType.Transparent;
                blendMode = BlendMode.Alpha;
            }
            material.SetFloat( "_Surface", (float)surfaceType );
            material.SetFloat( "_Blend", (float)blendMode );

            MaterialChanged( material );
        }

    }
}
