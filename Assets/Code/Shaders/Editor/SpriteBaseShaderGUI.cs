using UnityEngine;
using UnityEditor.AnimatedValues;
using System;

namespace UnityEditor.Rendering.Universal.ShaderGUI
{
    // At least as of URP 10.5.0, the material editor for sprites seems
    // to just be a generic, generated editor, that unceremoniously 
    // barfs all of the properties into the inspector.
    // 
    // I want my sprite materials to have some decent usability though,
    // and that requires custom editors. This serves as a base, essentially 
    // a copy of Unity's BaseShaderGUI, adapted for use with sprites.
    internal abstract class SpriteBaseShaderGUI : UnityEditor.ShaderGUI
    {
        [Flags]
        protected enum Expandable
        {
            SurfaceOptions = 1 << 0,
            SurfaceInputs = 1 << 1,
            Advanced = 1 << 2,
            Details = 1 << 3,
        }

        protected MaterialEditor materialEditor { get; set; }

        protected MaterialProperty p_mainTexture;

        protected GUIContent l_mainTexture = new GUIContent( "Base Texture" );

        public bool m_FirstTimeApply = true;

        private const string k_KeyPrefix = "UniversalRP:Material:UI_State:";

        private string m_HeaderStateKey = null;
        protected string headerStateKey { get { return m_HeaderStateKey; } }

        SavedBool m_SurfaceOptionsFoldout;
        SavedBool m_SurfaceInputsFoldout;
        SavedBool m_AdvancedFoldout;


        public abstract void MaterialChanged( Material material );

        public virtual void FindProperties( MaterialProperty[] properties )
        {
            p_mainTexture = FindProperty( DQUProperties.MainTextureMap, properties );
        }


        public override void OnGUI( MaterialEditor materialEditorIn, MaterialProperty[] properties )
        {
            if( materialEditorIn == null )
                throw new ArgumentNullException( typeof(SpriteBaseShaderGUI).ToString() + ": Material Editor argument is null." );

            FindProperties( properties );   // MaterialProperties can be animated so we do not cache them but fetch them every event to ensure animated values are updated correctly
            
            materialEditor = materialEditorIn;
            Material material = materialEditor.target as Material;

            if( m_FirstTimeApply )
            {
                OnOpenGUI( material, materialEditorIn );
                m_FirstTimeApply = false;
            }

            ShaderPropertiesGUI( material );
        }


        public virtual void OnOpenGUI( Material material, MaterialEditor materialEditor )
        {
            // Foldout states
            m_HeaderStateKey = k_KeyPrefix + material.shader.name; // Create key string for editor prefs
            m_SurfaceOptionsFoldout = new SavedBool( $"{m_HeaderStateKey}.SurfaceOptionsFoldout", true );
            m_SurfaceInputsFoldout = new SavedBool( $"{m_HeaderStateKey}.SurfaceInputsFoldout", true );
            m_AdvancedFoldout = new SavedBool( $"{m_HeaderStateKey}.AdvancedFoldout", false );

            foreach( var obj in materialEditor.targets )
                MaterialChanged( (Material)obj );
        }


        public void ShaderPropertiesGUI( Material material )
        {
            if( material == null )
                throw new ArgumentNullException( "material" );

            EditorGUI.BeginChangeCheck();

            m_SurfaceOptionsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup( m_SurfaceOptionsFoldout.value, DQUStyles.SurfaceOptions );
            if( m_SurfaceOptionsFoldout.value )
            {
                DrawSurfaceOptions( material );
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            m_SurfaceInputsFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup( m_SurfaceInputsFoldout.value, DQUStyles.SurfaceInputs );
            if( m_SurfaceInputsFoldout.value )
            {
                DrawSurfaceInputs( material );
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            DrawAdditionalFoldouts( material );

            m_AdvancedFoldout.value = EditorGUILayout.BeginFoldoutHeaderGroup( m_AdvancedFoldout.value, DQUStyles.AdvancedLabel );
            if( m_AdvancedFoldout.value )
            {
                DrawAdvancedOptions( material );
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            if( EditorGUI.EndChangeCheck() )
            {
                foreach( var obj in materialEditor.targets )
                    MaterialChanged( (Material)obj );
            }
        }


        #region DrawingFunctions

        public virtual void DrawSurfaceOptions( Material material ) { }

        public virtual void DrawSurfaceInputs( Material material )
        {
            DrawBaseProperties( material );
        }

        public virtual void DrawAdvancedOptions( Material material )
        {
            materialEditor.RenderQueueField();
            materialEditor.EnableInstancingField();
            materialEditor.DoubleSidedGIField();
        }


        public virtual void DrawAdditionalFoldouts( Material material ) { }

        public virtual void DrawBaseProperties( Material material )
        {
            if( p_mainTexture != null  )
                materialEditor.TextureProperty( p_mainTexture, "Base Texture" );
        }

        #endregion

    }
}
