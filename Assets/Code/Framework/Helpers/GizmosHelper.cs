using UnityEngine;

public static class GizmosHelper
{
    /// <summary>The color Unity seems to use to draw Collider gizmos.</summary>
    public static readonly Color ColliderColor = new Color( 198f / 255f, 250f / 255f, 195f / 255f, 0.5f ).linear;

    /// <summary>
    /// Draw a simple wireframe crosshairs at the specified position.
    /// </summary>
    public static void DrawCrosshairs2D( Vector3 position, float size )
    {
        Gizmos.DrawLine( position - new Vector3( size, 0, 0 ),
                         position + new Vector3( size, 0, 0 ) );
        Gizmos.DrawLine( position - new Vector3( 0, size, 0 ),
                         position + new Vector3( 0, size, 0 ) );
    }

    /// <summary>
    /// Draw a wireframe rhombus (a square rotated 45° really) at the specified position.
    /// </summary>
    public static void DrawShapeDiamond2D( Vector3 position, float size )
    {
        position.z -= 1;
        Vector3 a = position - new Vector3( size, 0.0f, 0 );
        Vector3 b = position + new Vector3( 0.0f, size, 0 );
        Vector3 c = position + new Vector3( size, 0.0f, 0 );
        Vector3 d = position - new Vector3( 0.0f, size, 0 );
        Gizmos.DrawLine( a, b );
        Gizmos.DrawLine( b, c );
        Gizmos.DrawLine( c, d );
        Gizmos.DrawLine( d, a );
    }

    /// <summary>
    /// Draw a wireframe rectangle at the specified position.
    /// </summary>
    public static void DrawShapeRectangle2D( Vector3 centerPosition, Vector2 size )
    {
        centerPosition.z -= 1;
        Vector2 halfSize = size / 2f;
        Vector3 a = centerPosition - (Vector3)halfSize;
        Vector3 b = centerPosition + new Vector3( -halfSize.x, halfSize.y, 0 );
        Vector3 c = centerPosition + (Vector3)halfSize;
        Vector3 d = centerPosition + new Vector3( halfSize.x, -halfSize.y, 0 );
        Gizmos.DrawLine( a, b );
        Gizmos.DrawLine( b, c );
        Gizmos.DrawLine( c, d );
        Gizmos.DrawLine( d, a );
    }

}