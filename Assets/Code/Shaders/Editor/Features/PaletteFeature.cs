using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using static UnityEditor.BaseShaderGUI;
using DQU.Editor;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    // Working with custom Unity editors is a pain. The purpose of these 
    // "feature" classes is to reduce repetitive code and improve organization.

    /// <summary>
    /// Used by Material Editors.
    /// Helps draw Editor fields and maintain state for palette-style color.
    /// </summary>
    internal class PaletteFeature
    {
        // Properties
        protected MaterialProperty p_hasColor2, p_hasColor3, p_hasColor4;
        protected MaterialProperty p_color1High, p_color1Low,
                                   p_color2High, p_color2Low,
                                   p_color3High, p_color3Low,
                                   p_color4High, p_color4Low;

        // GUI Content Labels
        protected GUIContent l_color1 = new GUIContent( "Color 1" ),
                             l_color2 = new GUIContent( "Color 2" ),
                             l_color3 = new GUIContent( "Color 3" ),
                             l_color4 = new GUIContent( "Color 4" );
        protected GUIContent l_bright = new GUIContent( "Bright" ),
                             l_dark = new GUIContent( "Dark" );


        public void FindProperties( MaterialProperty[] properties )
        {
            p_hasColor2 = FindProperty( DQUProperties.HasColor2, properties );
            p_hasColor3 = FindProperty( DQUProperties.HasColor3, properties );
            p_hasColor4 = FindProperty( DQUProperties.HasColor4, properties );

            p_color1High = FindProperty( DQUProperties.Color1High, properties );
            p_color1Low  = FindProperty( DQUProperties.Color1Low, properties );
            p_color2High = FindProperty( DQUProperties.Color2High, properties );
            p_color2Low  = FindProperty( DQUProperties.Color2Low, properties );
            p_color3High = FindProperty( DQUProperties.Color3High, properties );
            p_color3Low  = FindProperty( DQUProperties.Color3Low, properties );
            p_color4High = FindProperty( DQUProperties.Color4High, properties );
            p_color4Low  = FindProperty( DQUProperties.Color4Low, properties );
        }


        public void SetMaterialKeywords( Material material )
        {
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


        public void DrawSurfaceInputs( Material material )
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


        private bool DrawTogglableColorField(
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



    }
}
