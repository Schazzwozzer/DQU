
using UnityEngine;
using UnityEditor;
using System;
using ValueType = DQU.Editor.EditorHelper.ValueType;
using MaterialValueType = DQU.Editor.MaterialEditorHelper.ValueType;

namespace DQU.Editor
{
    // This class exists really just to keep EditorHelper a bit cleaner.
    // These methods are quite verbose

    /// <summary>
    /// Static class containing methods for drawing multiple properties on a single Editor line.
    /// </summary>
    static public class MultiValueFields
    {
        /// <summary>
        /// <para>Draw three Property fields on a single line.</para>
        /// Each property will take up about a third of the available space.
        /// </summary>
        public static void DrawThreeValueFieldAutoWidth( 
            Rect position, GUIContent label, float labelWidth, bool lowWidth,
            GUIContent labelOne, SerializedProperty propertyOne,
            GUIContent labelTwo, SerializedProperty propertyTwo,
            GUIContent labelThree, SerializedProperty propertyThree, ValueType valueType )
        {
            float fieldWidth = (position.width / 3f) - labelWidth - 2f - 2f;

            DrawThreeValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, fieldWidth,
                labelOne, propertyOne,
                labelTwo, propertyTwo,
                labelThree, propertyThree,
                valueType );
        }

        /// <summary>
        /// <para>Draw three Property fields on a single line.</para>
        /// The pixel width of each field is manually specified, 
        /// and the programmer must ensure it fits within the GUI.
        /// </summary>
        public static void DrawThreeValueFieldManualWidth(
            Rect position, GUIContent label, float labelWidth,
            bool lowWidth, float fieldWidth,
            GUIContent labelOne, SerializedProperty propertyOne,
            GUIContent labelTwo, SerializedProperty propertyTwo, 
            GUIContent labelThree, SerializedProperty propertyThree, ValueType valueType )
        {
            position = DrawPrefixLabel( position, label, lowWidth );

            // I dunno why this is necessary, but if this part of the drawer executes with an 
            // indent level, each field will be indented, totally throwing off the layout.
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if( propertyOne != null )
            {
                position = DrawValueField(
                    position, labelWidth, labelOne,
                    fieldWidth, propertyOne, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            if( propertyTwo != null )
            {
                position = PositionForwardAndUpdateWidth( position, labelWidth );
                position = DrawValueField(
                    position, labelWidth, labelTwo,
                    fieldWidth, propertyTwo, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            if( propertyThree != null )
            {
                position = PositionForwardAndUpdateWidth( position, labelWidth );
                position = DrawValueField(
                    position, labelWidth, labelThree,
                    fieldWidth, propertyThree, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            // Reset indent level
            EditorGUI.indentLevel = oldIndentLevel;
        }

        /// <summary>
        /// <para>Draw two Property fields on a single line.</para>
        /// Each property will take up about half of the available space.
        /// </summary>
        public static void DrawTwoValueFieldAutoWidth( 
            Rect position, GUIContent label, float labelWidth, bool lowWidth,
            GUIContent labelOne, SerializedProperty propertyOne,
            GUIContent labelTwo, SerializedProperty propertyTwo, ValueType valueType )
        {
            float fieldWidth = (position.width / 2f) - labelWidth - 2f - 2f;

            DrawTwoValueFieldManualWidth(
                position, label, labelWidth, 
                lowWidth, fieldWidth,
                labelOne, propertyOne,
                labelTwo, propertyTwo,
                valueType );
        }

        /// <summary>
        /// <para>Draw two Property fields on a single line.</para>
        /// The pixel width of each field is manually specified, 
        /// and the programmer must ensure it fits within the GUI.
        /// </summary>
        public static void DrawTwoValueFieldManualWidth(
            Rect position, GUIContent label, float labelWidth, 
            bool lowWidth, float fieldWidth,
            GUIContent labelOne, SerializedProperty propertyOne,
            GUIContent labelTwo, SerializedProperty propertyTwo, ValueType valueType )
        {
            position = DrawPrefixLabel( position, label, lowWidth );

            // I dunno why this is necessary, but if this part of the drawer executes with an 
            // indent level, each field will be indented, totally throwing off the layout.
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if( propertyOne != null )
            {
                position = DrawValueField(
                    position, labelWidth, labelOne,
                    fieldWidth, propertyOne, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            if( propertyTwo != null )
            {
                position = PositionForwardAndUpdateWidth( position, labelWidth );
                position = DrawValueField(
                    position, labelWidth, labelTwo,
                    fieldWidth, propertyTwo, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            // Reset indent level
            EditorGUI.indentLevel = oldIndentLevel;
        }


        private static Rect DrawPrefixLabel( Rect position, GUIContent label, bool lowWidth )
        {
            if( lowWidth )
            {
                EditorGUI.PrefixLabel( position, label );
                // This is a faux-indentation. If we do a real indentation, there 
                // will be a weird gap between the two elements. Real nice, Unity!
                position = new Rect(
                    position.x + 15f,
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width - 15f,
                    position.height );
            }
            else
            {
                position = EditorGUI.PrefixLabel( position, label );
            }
            return position;
        }


        /// <summary>
        /// Draw a single prefix label and property field.
        /// </summary>
        private static Rect DrawValueField(
            Rect position,
            float labelWidth, GUIContent label,
            float fieldWidth, SerializedProperty property,
            ValueType valueType )
        {
            position = DrawControlLabelAndUpdatePosition( position, label, labelWidth, fieldWidth );
            switch( valueType )
            {
                case ValueType.Int: property.intValue = EditorGUI.IntField( position, property.intValue ); break;
                case ValueType.Float: property.floatValue = EditorGUI.FloatField( position, property.floatValue ); break;
                case ValueType.String: property.stringValue = EditorGUI.TextField( position, property.stringValue ); break;
                case ValueType.Enum: EditorGUI.PropertyField( position, property, GUIContent.none ); break;
                case ValueType.Color: property.colorValue = EditorGUI.ColorField( position, property.colorValue ); break;
                case ValueType.Bool: property.boolValue = EditorGUI.Toggle( position, property.boolValue ); break;
            }

            return position;
        }


        private static Rect DrawControlLabelAndUpdatePosition( Rect position, GUIContent label, float labelWidth, float fieldWidth )
        {
            position.width = labelWidth;
            EditorGUI.LabelField( position, label, GUIContent.none );

            return PositionForwardAndUpdateWidth( position, fieldWidth );
        }


        /// <summary>
        /// Call this after drawing a field, to make space for the next.
        /// <para>Move the rect to the right by its current width, and update its width to the specified value.</para>
        /// </summary>
        private static Rect PositionForwardAndUpdateWidth( Rect rect, float newWidth )
        {
            return new Rect( rect.x + rect.width + 2f, rect.y, newWidth, rect.height );
        }


        /************************************
         *        Material Editor
        //**********************************/
        #region Material Properties


        /// <summary>
        /// <para>Draw three Property fields on a single line.</para>
        /// Each property will take up about a third of the available space.
        /// </summary>
        public static void DrawThreeValueFieldAutoWidth(
            Rect position, GUIContent label, float labelWidth, bool lowWidth,
            GUIContent labelOne, MaterialProperty propertyOne,
            GUIContent labelTwo, MaterialProperty propertyTwo,
            GUIContent labelThree, MaterialProperty propertyThree, MaterialValueType valueType )
        {
            float fieldWidth = (position.width / 3f) - labelWidth - 2f - 2f;

            DrawThreeValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, fieldWidth,
                labelOne, propertyOne,
                labelTwo, propertyTwo,
                labelThree, propertyThree,
                valueType );
        }

        /// <summary>
        /// <para>Draw three Property fields on a single line.</para>
        /// The pixel width of each field is manually specified, 
        /// and the programmer must ensure it fits within the GUI.
        /// </summary>
        public static void DrawThreeValueFieldManualWidth(
            Rect position, GUIContent label, float labelWidth,
            bool lowWidth, float fieldWidth,
            GUIContent labelOne, MaterialProperty propertyOne,
            GUIContent labelTwo, MaterialProperty propertyTwo,
            GUIContent labelThree, MaterialProperty propertyThree, MaterialValueType valueType )
        {
            position = DrawPrefixLabel( position, label, lowWidth );

            // I dunno why this is necessary, but if this part of the drawer executes with an 
            // indent level, each field will be indented, totally throwing off the layout.
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if( propertyOne != null )
            {
                position = DrawValueField(
                    position, labelWidth, labelOne,
                    fieldWidth, propertyOne, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            if( propertyTwo != null )
            {
                position = PositionForwardAndUpdateWidth( position, labelWidth );
                position = DrawValueField(
                    position, labelWidth, labelTwo,
                    fieldWidth, propertyTwo, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            if( propertyThree != null )
            {
                position = PositionForwardAndUpdateWidth( position, labelWidth );
                position = DrawValueField(
                    position, labelWidth, labelThree,
                    fieldWidth, propertyThree, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            // Reset indent level
            EditorGUI.indentLevel = oldIndentLevel;
        }

        /// <summary>
        /// <para>Draw two Property fields on a single line.</para>
        /// Each property will take up about half of the available space.
        /// </summary>
        public static void DrawTwoValueFieldAutoWidth(
            Rect position, GUIContent label, float labelWidth, bool lowWidth,
            GUIContent labelOne, MaterialProperty propertyOne,
            GUIContent labelTwo, MaterialProperty propertyTwo, MaterialValueType valueType )
        {
            float fieldWidth = ((position.width - EditorGUIUtility.labelWidth ) / 2f) - labelWidth - 2f - 2f;

            DrawTwoValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, fieldWidth,
                labelOne, propertyOne,
                labelTwo, propertyTwo,
                valueType );
        }

        /// <summary>
        /// <para>Draw two Property fields on a single line.</para>
        /// The pixel width of each field is manually specified, 
        /// and the programmer must ensure it fits within the GUI.
        /// </summary>
        public static void DrawTwoValueFieldManualWidth(
            Rect position, GUIContent label, float labelWidth,
            bool lowWidth, float fieldWidth,
            GUIContent labelOne, MaterialProperty propertyOne,
            GUIContent labelTwo, MaterialProperty propertyTwo, MaterialValueType valueType )
        {
            position = DrawPrefixLabel( position, label, lowWidth );

            // I dunno why this is necessary, but if this part of the drawer executes with an 
            // indent level, each field will be indented, totally throwing off the layout.
            int oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            if( propertyOne != null )
            {
                position = DrawValueField(
                    position, labelWidth, labelOne,
                    fieldWidth, propertyOne, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            if( propertyTwo != null )
            {
                position = PositionForwardAndUpdateWidth( position, labelWidth );
                position = DrawValueField(
                    position, labelWidth, labelTwo,
                    fieldWidth, propertyTwo, valueType );
            }
            else
                position.x += labelWidth + 2f + fieldWidth + 2f;

            // Reset indent level
            EditorGUI.indentLevel = oldIndentLevel;
        }

        /// <summary>
        /// Draw a single prefix label and property field.
        /// </summary>
        private static Rect DrawValueField(
            Rect position,
            float labelWidth, GUIContent label,
            float fieldWidth, MaterialProperty property,
            MaterialValueType valueType )
        {
            position = DrawControlLabelAndUpdatePosition( position, label, labelWidth, fieldWidth );
            switch( valueType )
            {
                case MaterialValueType.Int: property.floatValue = EditorGUI.IntField( position, (int)property.floatValue ); break;
                case MaterialValueType.Float: property.floatValue = EditorGUI.FloatField( position, property.floatValue ); break;
                case MaterialValueType.Color: property.colorValue = EditorGUI.ColorField( position, property.colorValue ); break;
                case MaterialValueType.Bool: property.floatValue = EditorGUI.Toggle( position, property.floatValue > 0f ) ? 1f : 0f; break;
                default: UnityEngine.Debug.LogError( "Trying to draw Material Property with invalid value type (" + valueType.ToString() + ")" ); break;
            }

            return position;
        }

        #endregion

    }
}
