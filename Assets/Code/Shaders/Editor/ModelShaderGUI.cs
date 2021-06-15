using System;
using UnityEngine;
using UnityEngine.Rendering;
using DQU.Editor;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class ModelShaderGUI : BaseShaderGUI
    {
        // Properties
        protected MaterialProperty p_receiveLighting;
        protected MaterialProperty p_hasColor2, p_hasColor3, p_hasColor4;
        protected MaterialProperty p_color1High, p_color1Low,
                                   p_color2High, p_color2Low,
                                   p_color3High, p_color3Low,
                                   p_color4High, p_color4Low;
        protected MaterialProperty p_colorShadow;
        protected MaterialProperty p_fresnel;

        // GUI Content Labels
        protected GUIContent l_receiveLighting = new GUIContent( 
            "Receive Lighting", 
            "If toggled, off, this material will be \"unlit\" and not receive lighting or shadows." );
        protected GUIContent l_color1 = new GUIContent( "Color 1" ),
                             l_color2 = new GUIContent( "Color 2" ),
                             l_color3 = new GUIContent( "Color 3" ),
                             l_color4 = new GUIContent( "Color 4" );
        protected GUIContent l_bright = new GUIContent( "Bright" ),
                             l_dark = new GUIContent( "Dark" );
        protected GUIContent l_colorShadow  = new GUIContent( "Shadow Color" );
        protected GUIContent l_fresnel = new GUIContent( "Fresnel Outline" ),
                             l_bias    = new GUIContent( "Bias" ),
                             l_scale   = new GUIContent( "Scale" ),
                             l_power   = new GUIContent( "Power" ),
                             l_step    = new GUIContent( "Step Value" );


        public override void FindProperties( MaterialProperty[] properties )
        {
            base.FindProperties( properties );

            p_receiveLighting = FindProperty( DQUProperties.ReceiveLighting, properties );
            p_hasColor2 = FindProperty( DQUProperties.HasColor2, properties );
            p_hasColor3 = FindProperty( DQUProperties.HasColor3, properties );
            p_hasColor4 = FindProperty( DQUProperties.HasColor4, properties );

            p_color1High   = FindProperty( DQUProperties.Color1High,  properties );
            p_color1Low    = FindProperty( DQUProperties.Color1Low,   properties );
            p_color2High   = FindProperty( DQUProperties.Color2High,  properties );
            p_color2Low    = FindProperty( DQUProperties.Color2Low,   properties );
            p_color3High   = FindProperty( DQUProperties.Color3High,  properties );
            p_color3Low    = FindProperty( DQUProperties.Color3Low,   properties );
            p_color4High   = FindProperty( DQUProperties.Color4High,  properties );
            p_color4Low    = FindProperty( DQUProperties.Color4Low,   properties );

            p_colorShadow  = FindProperty( DQUProperties.ColorShadow, properties );

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

            if( material.HasProperty( DQUProperties.ReceiveLightingID ) )
                CoreUtils.SetKeyword( material, "_RECEIVE_LIGHTING", material.GetFloat( DQUProperties.ReceiveLightingID ) > 0.0f );

            bool hasColor4, hasColor3, hasColor2;
            if( material.HasProperty( DQUProperties.HasColor4ID ) && material.GetFloat( DQUProperties.HasColor4ID ) > 0.0f )
            {
                hasColor4 = true;
                hasColor3 = hasColor2 = false;
            }
            else if( material.HasProperty( DQUProperties.HasColor3ID ) && material.GetFloat( DQUProperties.HasColor3ID ) > 0.0f )
            {
                hasColor3 = true;
                hasColor4 = hasColor2 = false;
            }
            else if( material.HasProperty( DQUProperties.HasColor2ID ) && material.GetFloat( DQUProperties.HasColor2ID ) > 0.0f )
            {
                hasColor2 = true;
                hasColor4 = hasColor3 = false;
            }
            else
                hasColor4 = hasColor3 = hasColor2 = false;

            CoreUtils.SetKeyword( material, "_PALETTE_COUNT_4", hasColor4 );
            CoreUtils.SetKeyword( material, "_PALETTE_COUNT_3", hasColor3 );
            CoreUtils.SetKeyword( material, "_PALETTE_COUNT_2", hasColor2 );
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

                DrawFloatToggleProperty( l_receiveLighting, p_receiveLighting );
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
            DrawTileOffset( materialEditor, baseMapProp );
            
            DrawColorFields( material );

            DrawFresnelSettings( material );
        }


        protected void DrawColorFields( Material material )
        {
            EditorGUILayout.LabelField( new GUIContent( "Colors" ), CustomGUIStyles.LabelBold );

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel += 2;

            MaterialEditorHelper.DrawTwoValueFieldColor(
                EditorGUILayout.GetControlRect(),
                l_color1, 0f, false,
                GUIContent.none, p_color1High,
                GUIContent.none, p_color1Low );

            EditorGUI.indentLevel = oldIndent + 1;

            if( DrawTogglableColorField( l_color2, p_hasColor2, p_color2High, p_color2Low ) )
                if( DrawTogglableColorField( l_color3, p_hasColor3, p_color3High, p_color3Low ) )
                    DrawTogglableColorField( l_color4, p_hasColor4, p_color4High, p_color4Low );

            EditorGUI.indentLevel = oldIndent;
        }


        protected bool DrawTogglableColorField( 
            GUIContent label, 
            MaterialProperty hasColor,
            MaterialProperty colorHigh, 
            MaterialProperty colorLow )
        {
            Rect position = EditorGUILayout.GetControlRect();
            Rect togglePos = new Rect( position.position, new Vector2( EditorGUIUtility.labelWidth, position.height ) );
            GUIStyle style;
            if( hasColor.floatValue == 0.0f )
                style = CustomGUIStyles.Disabled;
            else
                style = GUIStyle.none;
            hasColor.floatValue = EditorGUI.ToggleLeft( togglePos, label, hasColor.floatValue > 0.0f, style ) ? 1.0f : 0.0f;
            if( hasColor.floatValue > 0.0 )
            {
                MaterialEditorHelper.DrawTwoValueFieldColor( position,
                new GUIContent( " " ), 0f, false,   // Using a dummy 'spacer' GUIContent
                GUIContent.none, colorHigh,
                GUIContent.none, colorLow );

                return true;
            }
            return false;
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

        // This method is copied directly from Unity's BaseShaderGUI.
        // They decided to mark it internal, 
        protected static void DrawFloatToggleProperty( GUIContent styles, MaterialProperty prop )
        {
            if( prop == null )
                return;

            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;
            bool newValue = EditorGUILayout.Toggle( styles, prop.floatValue == 1 );
            if( EditorGUI.EndChangeCheck() )
                prop.floatValue = newValue ? 1.0f : 0.0f;
            EditorGUI.showMixedValue = false;
        }

    }
}
