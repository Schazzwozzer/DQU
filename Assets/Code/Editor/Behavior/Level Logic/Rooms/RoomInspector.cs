using UnityEngine;
using UnityEditor;

namespace DQU.Editor
{
    [CustomEditor( typeof( Room ))]
    public class RoomInspector : EditorPlus
    {

        private SerializedProperty p_roomNumber;
        private SerializedProperty p_clampToBoundsX, p_clampToBoundsY;

        protected override void OnEnable()
        {
            base.OnEnable();

            p_roomNumber = serializedObject.FindProperty( "_roomNumber" );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawScriptField();

            EditorGUILayout.PropertyField( p_roomNumber );

            Rect position = EditorGUILayout.GetControlRect();

            DrawDefaultInspectorMinusScript();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
