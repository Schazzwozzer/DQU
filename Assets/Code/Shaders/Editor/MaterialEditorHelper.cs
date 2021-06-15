
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
        public enum ValueType { Int, Float, Color, Bool };


        public static void DrawThreeValueFieldInt( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                   GUIContent labelOne, MaterialProperty propertyOne,
                                                   GUIContent labelTwo, MaterialProperty propertyTwo,
                                                   GUIContent labelThree, MaterialProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.Int );
        }


        public static void DrawThreeValueFieldFloat( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                     GUIContent labelOne, MaterialProperty propertyOne,
                                                     GUIContent labelTwo, MaterialProperty propertyTwo,
                                                     GUIContent labelThree, MaterialProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.Float );
        }

        public static void DrawThreeValueFieldBool( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                    GUIContent labelOne, MaterialProperty propertyOne,
                                                    GUIContent labelTwo, MaterialProperty propertyTwo,
                                                    GUIContent labelThree, MaterialProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldManualWidth(
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
            MultiValueFields.DrawTwoValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Float );
        }

        public static void DrawTwoValueFieldInt( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                 GUIContent labelOne, MaterialProperty propertyOne,
                                                 GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldAutoWidth(
                position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Int );
        }

        public static void DrawTwoValueFieldColor( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                   GUIContent labelOne, MaterialProperty propertyOne,
                                                   GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldAutoWidth(
                position, label, labelWidth, lowWidth, 
                labelOne, propertyOne,
                labelTwo, propertyTwo, ValueType.Color );
        }

        public static void DrawTwoValueFieldBool( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                  GUIContent labelOne, MaterialProperty propertyOne,
                                                  GUIContent labelTwo, MaterialProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, 20f,
                labelOne, propertyOne,
                labelTwo, propertyTwo, ValueType.Bool );
        }

    }
}
