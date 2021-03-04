using UnityEngine;
using UnityEditor;

namespace DQU.Editor
{
    [CustomEditor( typeof( Room ))]
    public class RoomInspector : EditorPlus
    {

        private static GUIContent l_clampToBounds = new GUIContent( "Clamp to Room Bounds" ),
                                  l_x = new GUIContent( "X" ),
                                  l_y = new GUIContent( "Y" );

        private SerializedProperty p_roomNumber;
        private SerializedProperty p_clampToBoundsX, p_clampToBoundsY;

        protected override void OnEnable()
        {
            base.OnEnable();

            p_roomNumber = serializedObject.FindProperty( "_roomNumber" );
            p_clampToBoundsX = serializedObject.FindProperty( "_clampToBoundsX" );
            p_clampToBoundsY = serializedObject.FindProperty( "_clampToBoundsY" );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptField();

            EditorGUILayout.PropertyField( p_roomNumber );

            Rect position = EditorGUILayout.GetControlRect();
            DrawClampToBoundsField( position, serializedObject );

            DrawDefaultInspectorMinusScript();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draw the Room component's Clamp-To-Room-Bounds X and Y toggles on the same line.
        /// </summary>
        private void DrawClampToBoundsField( Rect position, SerializedObject serializedObject )
        {
            EditorHelper.DrawTwoValueFieldBool( 
                position, 
                l_clampToBounds, 12f, false, 
                l_x, p_clampToBoundsX, 
                l_y, p_clampToBoundsY );
        }
    }
}
