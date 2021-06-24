using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using static UnityEditor.BaseShaderGUI;
using DQU.Editor;
using Helper = DQU.Editor.MaterialEditorHelper;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    // Working with custom Unity editors is a pain. The purpose of these 
    // "feature" classes is to reduce repetitive code and improve organization.

    /// <summary>
    /// Used by Material Editors.
    /// Helps draw Editor fields and maintain state for lighting-specific inputs.
    /// </summary>
    internal class LightingFeature
    {
        // Properties
        protected MaterialProperty p_receiveLighting;
        protected MaterialProperty p_colorShadow;

        // GUI Content Labels
        protected GUIContent l_receiveLighting = new GUIContent(
            "Receive Lighting",
            "If toggled, off, this material will be \"unlit\" and not be subject to lighting or shadows." );
        protected GUIContent l_colorShadow = new GUIContent( "Shadow Color" );


        public void FindProperties( MaterialProperty[] properties )
        {
            p_receiveLighting = FindProperty( DQUProperties.ReceiveLighting, properties );
            p_colorShadow     = FindProperty( DQUProperties.ColorShadow, properties );
        }


        public void SetMaterialKeywords( Material material )
        {
            if( material.HasProperty( DQUProperties.ReceiveLightingID ) )
                CoreUtils.SetKeyword( material, "_RECEIVE_LIGHTING", material.GetFloat( DQUProperties.ReceiveLightingID ) > 0.0f );
        }


        public void DrawSurfaceOptions( Material material )
        {
            Helper.DrawFloatToggleProperty( l_receiveLighting, p_receiveLighting );
        }


    }
}
