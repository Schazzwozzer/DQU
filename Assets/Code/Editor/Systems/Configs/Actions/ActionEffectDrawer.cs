using UnityEngine;
using UnityEditor;
using DQU.Configurations;

namespace DQU.Editor
{
    [CustomPropertyDrawer( typeof( ActionEffect ))]
    public class ActionEffectDrawer : PropertyDrawer
    {
        private const string effectTypePropertyName = "_effectType";
        private const string magnitudePropertyName = "_magnitude";

        private static float postEffectTypePadding = 9f;


        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            EditorGUI.BeginProperty( position, label, property );
            {
                Rect contentRect = new Rect( position.x, position.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight );
                contentRect.width -= postEffectTypePadding;

                // Draw the dropdown 
                SerializedProperty p_effectType = property.FindPropertyRelative( effectTypePropertyName );
                EditorGUI.PropertyField( contentRect, p_effectType, GUIContent.none );

                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;

                position = EditorHelper.GetControlRect( position );

                SerializedProperty p_magnitude = property.FindPropertyRelative( magnitudePropertyName );
                EditorGUI.PropertyField( position, p_magnitude, GUIContent.none );

                EditorGUI.indentLevel = oldIndent;
            }
            EditorGUI.EndProperty();
        }
    }
}
