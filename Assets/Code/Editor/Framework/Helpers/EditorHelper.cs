
using UnityEngine;
using UnityEditor;
using System;

namespace DQU.Editor
{
    /// <summary>
    /// Static class containing methods to simplify the tedious process of generating custom Unity editor GUI.
    /// </summary>
    static public class EditorHelper
    {

        /// <summary>Simulate the "Header" attribute.</summary>
        public static void Header( string text )
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField( text, EditorStyles.boldLabel );
        }

        /// <summary>
        /// Calculate the pixel height required for the number of editor lines.
        /// </summary>
        public static float CalculateHeight( int numberOfLines )
        {
            return EditorGUIUtility.singleLineHeight * numberOfLines +
                   EditorGUIUtility.standardVerticalSpacing * Math.Max( 0, numberOfLines - 1 );
        }


        public static bool IsLowWidth()
        {
            return Screen.width < 333;
        }


        /// <summary>
        /// Draw a Mask Field that, unlike Unity's MaskField methods, 
        /// functions appropriately with enums that start at 0.
        /// </summary>
        /// <param name="position">Rectangle on the screen to use for this control.</param>
        /// <param name="maskValue">The current mask to display.</param>
        /// <param name="type">The enum type of this mask.</param>
        /// <param name="label">Label for the field.</param>
        public static int DrawBitMaskField( Rect position, int maskValue, System.Type type, GUIContent label )
        {
            var itemNames = System.Enum.GetNames( type );
            var itemValues = System.Enum.GetValues( type ) as int[];

            // I don't quite understand this initial for-loop, but I
            // think it's recompiling the property's underlying value, with 
            // special fixes for the off-by-1 nature of Unity's MaskField solution.
            int val = maskValue;
            int maskVal = 0;
            for( int i = 0; i < itemValues.Length; i++ )
            {
                if( itemValues[i] != 0 )
                {
                    // If the incoming mask value had this flag, also add it to maskVal
                    if( (val & itemValues[i]) == itemValues[i] )
                        maskVal |= 1 << i;
                }
                else if( val == 0 )
                    // If the incoming value is 0, go ahead 
                    maskVal |= 1 << i;
            }
            // Draw a standard mask field using maskVal and get the user's input as a new value
            int newMaskVal = EditorGUI.MaskField( position, label, maskVal, itemNames );
            // Exclusive OR operator, results in differences between two values
            int changes = maskVal ^ newMaskVal;

            for( int i = 0; i < itemValues.Length; i++ )
            {
                if( (changes & (1 << i)) != 0 )         // has this list item changed?
                {
                    if( (newMaskVal & (1 << i)) != 0 )  // has it been set?
                    {
                        if( itemValues[i] == 0 )            // special case: if "0" is set, just set the val to 0
                        {
                            val = 0;
                            break;
                        }
                        else
                            val |= itemValues[i];
                    }
                    else                                    // it has been reset
                    {
                        val &= ~itemValues[i];
                    }
                }
            }
            return val;
        }


        public static float GetControlWidth( Rect position )
        {
            return position.width - EditorGUIUtility.labelWidth - 15f;
        }

        /// <summary>
        /// Works similarly to EditorGUILayout.GetControlRect, but that method 
        /// 'claims' a vertical line in the Editor, perhaps indicating that it was 
        /// intended to be used only as an argument WITHIN a DrawPropertyField().
        /// <para>This version does not claim a line, and can be freely used for other calculations.</para>
        /// </summary>
        /// <param name="position"></param>
        public static Rect GetControlRect( Rect position )
        {
            return new Rect( position.x + EditorGUIUtility.labelWidth,
                             position.y,
                             position.width - EditorGUIUtility.labelWidth,
                             position.height );
            //return GUILayoutUtility.GetRect( ( !hasLabel ) ? EditorGUIUtility.fieldWidth : EditorGUILayout.kLabelFloatMinW, EditorGUILayout.kLabelFloatMaxW, height, height, style, options );
        }


        /// <summary>Update the position rect so that it occupies a new line.</summary>
        public static void NextLine( ref Rect position, int lines = 1 )
        {
            position.y += (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * lines;
        }


        /// <summary>
        /// The pixel width required for the circle buttons to the right of Unity object controls.
        /// </summary>
        public static float selectorButtonWidth { get { return 18f; } }


        public static GUIContent moveButtonContent = new GUIContent( "\u21b4", "move down" ),
                                 duplicateButtonContent = new GUIContent( "+", "duplicate" ),
                                 deleteButtonContent = new GUIContent( "-", "delete" ),
                                 addButtonContent = new GUIContent( "+", "add element" );
        private static GUILayoutOption miniButtonWidth = GUILayout.Width( 20f );


        #region Recreation of Standard Editor Controls

        /// <summary>
        /// Recreate the standard Unity Editor array field, using manual layout.
        /// </summary>
        public static Rect ArrayField(
            Rect position,
            SerializedObject serializedObject,
            SerializedProperty array,
            string propertyName,
            string elementName = "Element" )
        {
            array.isExpanded = EditorGUI.Foldout( position, array.isExpanded, new GUIContent( propertyName ) );
            if( array.isExpanded )
            {
                int originalindent = EditorGUI.indentLevel;
                ++EditorGUI.indentLevel;

                NextLine( ref position );
                array.arraySize = EditorGUI.IntField( position, "Size", array.arraySize );

                for( int i = 0; i < array.arraySize; ++i )
                {
                    NextLine( ref position );
                    EditorGUI.PropertyField(
                        position,
                        array.GetArrayElementAtIndex( i ),
                        new GUIContent( string.Format( "{0} {1}", elementName, i ) ),
                        true );
                }

                EditorGUI.indentLevel = originalindent;
            }
            serializedObject.ApplyModifiedProperties();

            NextLine( ref position );
            return position;
        }


        /// <summary>
        /// Recreate the standard Unity Editor array field, using auto layout.
        /// </summary>
        public static void ArrayFieldLayout(
            SerializedObject serializedObject,
            SerializedProperty array,
            string propertyName,
            string elementName = "Element" )
        {
            array.isExpanded = EditorGUILayout.Foldout( array.isExpanded, new GUIContent( propertyName ) );
            if( array.isExpanded )
            {
                int originalindent = EditorGUI.indentLevel;
                ++EditorGUI.indentLevel;

                array.arraySize = EditorGUILayout.IntField( "Size", array.arraySize );

                for( int i = 0; i < array.arraySize; ++i )
                {
                    EditorGUILayout.PropertyField(
                        array.GetArrayElementAtIndex( i ),
                        new GUIContent( string.Format( "{0} {1}", elementName, i ) ),
                        true );
                }

                EditorGUI.indentLevel = originalindent;
            }
            serializedObject.ApplyModifiedProperties();
        }


        public static void DrawListButtons( SerializedProperty list, int index )
        {
            if( GUILayout.Button( moveButtonContent, EditorStyles.miniButtonLeft, miniButtonWidth ) )
                list.MoveArrayElement( index, index + 1 );

            if( GUILayout.Button( duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth ) )
                list.InsertArrayElementAtIndex( index );

            if( GUILayout.Button( deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth ) )
            {
                // This goofiness is necessary because DeleteArrayElementAtIndex()
                // clears the reference but only removes the element if it's empty.
                int oldSize = list.arraySize;
                list.DeleteArrayElementAtIndex( index );
                if( list.arraySize == oldSize )
                    list.DeleteArrayElementAtIndex( index );
            }
        }


        #endregion


        /// <summary>
        /// Draw a button that occupies a full line in the editor. Returns a boolean describing whether or not the button was pressed.
        /// </summary>
        /// <param name="widthScalar">A value between 0 and 1.0 that determines how much of the line the button occupies.</param>
        /// <param name="fieldOnly">If true, the button will be offset on the left by prefix label width.</param>
        /// <returns>True if the button was pressed by the user, false if not.</returns>
        public static bool DrawCenteredButton( GUIContent label, float widthScalar, GUIStyle style = null, bool fieldOnly = false )
        {
            if( widthScalar <= 0 )
            {
                Debug.LogError( "Custom editor tried to draw a centered button with invalid width of " + widthScalar * 100f + "%" );
                widthScalar = 1.0f;
            }

            float margin = (1.0f - widthScalar) / 2f;
            // Make the button about 80% width and centered.
            Rect position = EditorGUILayout.GetControlRect();
            if( fieldOnly )
                position = GetControlRect( position );
            position.x += position.width * margin;
            position.width *= widthScalar;

            if( style == null )
                return UnityEngine.GUI.Button( position, label );
            else
                return UnityEngine.GUI.Button( position, label, style );
        }


        public enum ValueType { Int, Float, String, Color, Enum, Bool };


        public static void DrawThreeValueFieldInt( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo,
                                                GUIContent labelThree, SerializedProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.Int );
        }


        public static void DrawThreeValueFieldFloat( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo,
                                                GUIContent labelThree, SerializedProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.Float );
        }


        public static void DrawThreeValueFieldString( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo,
                                                GUIContent labelThree, SerializedProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, labelThree, propertyThree, ValueType.String );
        }

        public static void DrawThreeValueFieldBool( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                  GUIContent labelOne, SerializedProperty propertyOne,
                                                  GUIContent labelTwo, SerializedProperty propertyTwo,
                                                  GUIContent labelThree, SerializedProperty propertyThree )
        {
            MultiValueFields.DrawThreeValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, 20f,
                labelOne, propertyOne,
                labelTwo, propertyTwo,
                labelThree, propertyThree, ValueType.Bool );
        }



        
        public static void DrawTwoValueFieldFloat( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Float );
        }

        public static void DrawTwoValueFieldInt( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldAutoWidth( 
                position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Int );
        }

        public static void DrawTwoValueFieldString( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldAutoWidth( 
                position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.String );
        }

        public static void DrawTwoValueFieldEnum( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                GUIContent labelOne, SerializedProperty propertyOne,
                                                GUIContent labelTwo, SerializedProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldAutoWidth( position, label, labelWidth, lowWidth,
                labelOne, propertyOne, labelTwo, propertyTwo, ValueType.Enum );
        }

        public static void DrawTwoValueFieldBool( Rect position, GUIContent label, float labelWidth, bool lowWidth,
                                                  GUIContent labelOne, SerializedProperty propertyOne,
                                                  GUIContent labelTwo, SerializedProperty propertyTwo )
        {
            MultiValueFields.DrawTwoValueFieldManualWidth(
                position, label, labelWidth,
                lowWidth, 20f,
                labelOne, propertyOne,
                labelTwo, propertyTwo, ValueType.Bool );
        }







    }

}