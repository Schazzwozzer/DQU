
using UnityEngine;
using UnityEditor;

/// <summary>
/// Provides access to, as the name suggests, some custom GUI styles.
/// </summary>
public static class CustomGUIStyles
{
    private static GUIStyle _labelBold   = null;
    private static GUIStyle _divider     = null;
    private static GUIStyle _disabled    = null;
    private static GUIStyle _foldoutBold = null;
    private static GUIStyle _highlightBG = null;
    private static GUIStyle _highlightBG2 = null;
    private static GUIStyle _bgAlternate = null;

    // Static constructor, called "before any instance constructor is invoked or member is accessed."
    static CustomGUIStyles()
    {
        _labelBold = new GUIStyle( "label" );
        _labelBold.fontStyle = FontStyle.Bold;

        _divider = new GUIStyle( "box" );
        _divider.border.top = _divider.border.bottom = 1;
        _divider.margin.top = _divider.margin.bottom = 1;
        _divider.padding.top = _divider.padding.bottom = 1;

        _disabled = new GUIStyle( "label" );
        _disabled.normal.textColor = new Color( 0.4f, 0.4f, 0.4f );

        _foldoutBold = new GUIStyle( "foldout" );
        _foldoutBold.fontStyle = FontStyle.Bold;

        _highlightBG = new GUIStyle();
        Texture2D yellow = Resources.Load<Texture2D>( "Dev/Textures/Solid Colors/yellow" );
        _highlightBG.active.background = yellow;
        _highlightBG.normal.textColor = new Color( 1.0f, 0.5f, 1.0f );
        _highlightBG.normal.background = yellow;
        _highlightBG.border = _highlightBG.margin = _highlightBG.padding = new RectOffset( 0, 0, 0, 0 );

        _highlightBG2 = new GUIStyle();
        Texture2D red = Resources.Load<Texture2D>( "Dev/Textures/Solid Colors/red" );
        _highlightBG.active.background = red;
        _highlightBG2.normal.textColor = Color.white;
        _highlightBG2.normal.background = red;
        _highlightBG2.border = _highlightBG2.margin = _highlightBG2.padding = new RectOffset( 0, 0, 0, 0 );

        _bgAlternate = new GUIStyle( EditorStyles.label );
        Texture2D gray68 = Resources.Load<Texture2D>( "Dev/Textures/Solid Colors/gray_68" );
        _bgAlternate.active.background = gray68;
        _bgAlternate.normal.background = gray68;
        _bgAlternate.overflow = new RectOffset( 4, 4, 0, 0 );
        //bgAlternate.border = new RectOffset( -4, -4, 0, 0 );
        _bgAlternate.margin = _bgAlternate.padding = new RectOffset( 0, 0, 0, 0 );
        _bgAlternate.contentOffset = Vector2.zero;
        _bgAlternate = new GUIStyle();
    }

    /// <summary>The "label" GUIStyle with bold text.</summary>
    public static GUIStyle LabelBold    { get { return _labelBold; } }

    public static GUIStyle Divider      { get { return _divider; } }

    /// <summary>The "label" GUIStyle with gray text.</summary>
    public static GUIStyle Disabled     { get { return _disabled; } }

    /// <summary>The "foldout" GUIStyle with bold text.</summary>
    public static GUIStyle FoldoutBold  { get { return _foldoutBold; } }

    /// <summary>Standard GUIStyle with yellow background.</summary>
    public static GUIStyle HighlightBG { get { return _highlightBG; } }
    /// <summary>Standard GUIStyle with red background.</summary>
    public static GUIStyle HighlightBG2 { get { return _highlightBG2; } }

    /// <summary>
    /// Standard GUIStyle with a slightly darker toned background.
    /// Useful for differentiating elements in an array.
    /// </summary>
    public static GUIStyle BGAlternate { get { return _bgAlternate; } }


}