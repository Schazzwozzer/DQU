
using UnityEngine;
using UnityEditor;
using System;

/// <summary>
/// A helper class that extends Editor, adding in some common-use methods.
/// For instance, with a couple method calls, one can insert their own Property Fields after
/// the Script field but before the rest of the default inspector fields.
/// </summary>
public class EditorPlus : Editor
{
    protected SerializedProperty ScriptProperty;
    protected const string scriptPropertyName = "m_Script";

    /// <summary>Define <see cref="ScriptProperty"/>.</summary>
    protected virtual void OnEnable()
    {
        ScriptProperty = serializedObject.FindProperty( scriptPropertyName );
    }

    /// <summary>
    /// Draw the "Script" property field that is at the top of most Inspectors.
    /// As established by Unity, the field will be disabled (read only).
    /// </summary>
    public void DrawScriptField()
    {
        EditorGUI.BeginDisabledGroup( true );
        EditorGUILayout.PropertyField( ScriptProperty, true, new GUILayoutOption[0] );
        EditorGUI.EndDisabledGroup();
    }

    /// <summary>
    /// Draw all of the default properties, excluding the Script field.
    /// </summary>
    /// <returns></returns>
    public bool DrawDefaultInspectorMinusScript()
    {
        EditorGUI.BeginChangeCheck();

        serializedObject.Update();
        SerializedProperty iterator = serializedObject.GetIterator();
        bool enterChildren = true;
        bool scriptFound = false;
        while( iterator.NextVisible( enterChildren ) )
        {
            if( !scriptFound && iterator.name == scriptPropertyName )
            {
                // Skip the property if it's the Script property.
                scriptFound = true;
                continue;
            }
            EditorGUILayout.PropertyField( iterator, true, new GUILayoutOption[0] );
            enterChildren = false;
        }
        serializedObject.ApplyModifiedProperties();

        return EditorGUI.EndChangeCheck();
    }

    /// <summary>
    /// When drawing a property that is not serialized, it will not automatically 
    /// be updated. This method will set the Serialized Object as dirty.
    /// </summary>
    protected void ForceRedraw()
    {
        if( Application.isPlaying )
        {
            EditorUtility.SetDirty( target );
        }
    }

}
