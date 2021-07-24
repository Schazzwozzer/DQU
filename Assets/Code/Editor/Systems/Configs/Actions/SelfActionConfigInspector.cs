using UnityEngine;
using UnityEditor;
using DQU.Configurations;

namespace DQU.Editor
{
    [CustomEditor( typeof( SelfActionConfig ) ), CanEditMultipleObjects]
    public class SelfActionConfigInspector : EditorPlus
    {
        protected SerializedProperty p_defense,
                                   p_time,
                                   p_fatigueCost,
                                   p_selfEffects;

        protected override void OnEnable()
        {
            base.OnEnable();

            p_defense = serializedObject.FindProperty( "_defense" );
            p_time = serializedObject.FindProperty( "_time" );
            p_fatigueCost = serializedObject.FindProperty( "_fatigueCost" );
            p_selfEffects = serializedObject.FindProperty( "_selfEffects" );
        }

        public override void OnInspectorGUI()
        {
            DrawScriptField();

            EditorGUI.BeginChangeCheck();

            DrawProperties();    

            if( EditorGUI.EndChangeCheck() )
                serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draw fields for Defense, Time, Fatigue Cost, and Self Effects properties.
        /// </summary>
        protected void DrawProperties()
        {
            EditorGUILayout.PropertyField( p_defense );
            EditorGUILayout.PropertyField( p_time );
            EditorGUILayout.PropertyField( p_fatigueCost );
            EditorGUILayout.PropertyField( p_selfEffects );
        }

    }
}
