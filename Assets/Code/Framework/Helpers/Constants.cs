using UnityEngine;

/// <summary>
/// Helper class that makes constant values universally accessible.
/// </summary>
public static class Constants
{

    /// <summary>
    /// The number of pixels in a single unit/tile.
    /// </summary>
    public static int PixelsPerUnit { get { return 32; } }


    /// <summary>
    /// The size of a single pixel in world space.
    /// </summary>
    public static float PixelSize { get; private set; }

    /// <summary>
    /// Half the size of a single pixel in world space.
    /// </summary>
    public static float HalfPixelSize { get; private set; }


    static Constants()
    {
        PixelSize = 1f / PixelsPerUnit;
        HalfPixelSize = PixelSize * 0.5f;
    }

}
