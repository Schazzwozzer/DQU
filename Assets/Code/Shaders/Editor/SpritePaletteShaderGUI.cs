using System;
using UnityEngine;
using UnityEngine.Rendering;
using DQU.Editor;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    internal class SpritePaletteShaderGUI : SpriteBaseShaderGUI
    {
        protected LightingFeature _lightingFeature = new LightingFeature();
        protected PaletteFeature _paletteFeature = new PaletteFeature();


        public override void FindProperties( MaterialProperty[] properties )
        {
            base.FindProperties( properties );

            _lightingFeature.FindProperties( properties );
            _paletteFeature.FindProperties( properties );
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
        }


    }
}
