using UnityEngine;
using UnityEditor;
using System;

namespace DQU.Editor
{
    /// <summary>
    /// Static class containing methods to simplify the creation of Material editor GUI.
    /// <para>Separate from <see cref="EditorHelper"/>, because Materials deal with MaterialProperties rather than SerializedProperties.</para>
    /// </summary>
    static public class MaterialEditorHelper
    {

        // This method is copied directly from Unity's BaseShaderGUI. They decided to mark it internal.
        /// <summary>
        /// Materials can not have boolean properties, so this method 
        /// will draw a Toggle field that uses a float property as a boolean.
        /// </summary>
        public static void DrawFloatToggleProperty( GUIContent styles, MaterialProperty prop )
        {
            if( prop == null )
                return;

            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = prop.hasMixedValue;
            bool newValue = EditorGUILayout.Toggle( styles, prop.floatValue == 1 );
            if( EditorGUI.EndChangeCheck() )
                prop.floatValue = newValue ? 1.0f : 0.0f;
            EditorGUI.showMixedValue = false;
        }


        #region Multi-value fields

        public enum ValueType { Int, Float, Color, Bool };


        public static void DrawThreeValueFieldInt( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                   GUIContent labelOne, MaterialProperty propertyOne,
                                                   GUIContent labelTwo, MaterialProperty propertyTwo,
                                                   GUIContent labelThree, MaterialProperty propertyThree )
        {
            MultiValueFieldsMaterial.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.Int );
        }


        public static void DrawThreeValueFieldFloat( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                     GUIContent labelOne, MaterialProperty propertyOne,
                                                     GUIContent labelTwo, MaterialProperty propertyTwo,
                                                     GUIContent labelThree, MaterialProperty propertyThree )
        {
            MultiValueFieldsMaterial.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.Float );
        }

        public static void DrawThreeValueFieldBool( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                    GUIContent labelOne, MaterialProperty propertyOne,
                                                    GUIContent labelTwo, MaterialProperty propertyTwo,
                                                    GUIContent labelThree, MaterialProperty propertyThree )
        {
            MultiValueFieldsMaterial.DrawThreeValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, 20f,
                labelOne, propertyOne,
                labelTwo, propertyTwo,
                labelThree, propertyThree, ValueType.Bool );
        }




        public static void DrawTwoValueFieldFloat( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                   GUIContent labelOne, MaterialProperty propertyOne,
                                                   GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFieldsMaterial.DrawTwoValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Float );
        }

        public static void DrawTwoValueFieldInt( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                 GUIContent labelOne, MaterialProperty propertyOne,
                                                 GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFieldsMaterial.DrawTwoValueFieldAutoWidth(
                position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Int );
        }

        public static void DrawTwoValueFieldColor( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                   GUIContent labelOne, MaterialProperty propertyOne,
                                                   GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFieldsMaterial.DrawTwoValueFieldAutoWidth(
                position, label, labelWidth, lowWidth, 
                labelOne, propertyOne,
                labelTwo, propertyTwo, ValueType.Color );
        }

        public static void DrawTwoValueFieldBool( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                  GUIContent labelOne, MaterialProperty propertyOne,
                                                  GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFieldsMaterial.DrawTwoValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, 20f,
                labelOne, propertyOne,
                labelTwo, propertyTwo, ValueType.Bool );
        }

        #endregion

    }
}
