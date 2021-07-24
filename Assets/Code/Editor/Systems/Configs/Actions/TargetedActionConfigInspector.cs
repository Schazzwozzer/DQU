using UnityEngine;
using UnityEditor;
using DQU.Configurations;

namespace DQU.Editor
{
    [CustomEditor( typeof( TargetedActionConfig ) ), CanEditMultipleObjects]
    public class TargetedActionConfigInspector : SelfActionConfigInspector
    {
        protected SerializedProperty p_targetEffects;

        protected override void OnEnable()
        {
            base.OnEnable();

            p_targetEffects = serializedObject.FindProperty( "_targetEffects" );
        }

        public override void OnInspectorGUI()
        {
            DrawScriptField();

            EditorGUI.BeginChangeCheck();

            DrawProperties();
            DrawTargetedEffects();

            if( EditorGUI.EndChangeCheck() )
                serializedObject.ApplyModifiedProperties();
        }


        protected void DrawTargetedEffects()
        {
            EditorGUILayout.PropertyField( p_targetEffects );
        }

    }
}
