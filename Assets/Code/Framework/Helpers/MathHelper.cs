using UnityEngine;
using System;

public static class MathHelper
{
    /// <summary>Is this value divisible by 2 without remainder?</summary>
    public static bool IsEven( this int value )
    {
        return value % 2 == 0;
    }

    /// <summary>Does this value leave a remainder when divided by 2?</summary>
    public static bool IsOdd( this int value )
    {
        return !value.IsEven();
    }


    #region Rounding Methods

    // The following methods come from http://stackoverflow.com/questions/15154457/rounding-integers-to-nearest-multiple-of-10
    // and http://stackoverflow.com/questions/2705542/returning-the-nearest-multiple-value-of-a-number

    /// <summary>Round the "toRound" value up to the nearest multiple of the "multiple" value.</summary>
    public static int RoundUp( int toRound, int multiple )
    {
        return ((toRound + multiple - 1) / multiple) * multiple;
    }

    /// <summary>Round the "toRound" value down to the nearest multiple of the "multiple" value.</summary>
    public static int RoundDown( int toRound, int multiple )
    {
        //return toRound - toRound % multiple;
        // Without modulus...
        return multiple * (toRound / multiple);
    }

    /// <summary>Round the "toRound" value to the nearest multiple of the "multiple" value.</summary>
    public static int RoundToNearest( int toRound, int multiple )
    {
        return Mathf.RoundToInt( (float)toRound / (float)multiple ) * multiple;
    }

    /*** Float versions ***/
    /// <summary>Round the "toRound" value up to the nearest multiple of the "multiple" value.</summary>
    public static float RoundUp( float toRound, float multiple )
    {
        return ((toRound + multiple - 1) / multiple) * multiple;
    }

    /// <summary>Round the "toRound" value down to the nearest multiple of the "multiple" value.</summary>
    public static float RoundDown( float toRound, float multiple )
    {
        //return toRound - toRound % multiple;
        // Without modulus...
        return multiple * (toRound / multiple);
    }

    /// <summary>Round the "toRound" value to the nearest multiple of the "multiple" value.</summary>
    public static float RoundToNearest( float toRound, float multiple )
    {
        return (float)Math.Round( (toRound / multiple), MidpointRounding.AwayFromZero ) * multiple;
    }

    /// <summary>Halve the value, double it, and then round to nearest integer.</summary>
    public static int RoundToEvenInt( float value )
    {
        return Mathf.RoundToInt( value / 2f ) * 2;
    }

    /// <summary>Halve the value, double it, round it to its nearest integer, and then add 1.</summary>
    public static int RoundToOddInt( float value )
    {
        return RoundToEvenInt( value ) + 1;
    }

    #endregion


    public static int MoveTowards( int current, int target, int maxDelta )
    {
        if( Math.Abs( target - current ) <= maxDelta )
            return target;
        return current + Math.Sign( target - current ) * maxDelta;
    }

    /// <summary>
    /// <para>Calculate the position within an arbitrary range that
    /// a value resides, were it to loop around endlessly.</para>
    /// For example, in a range of 0—5, a value of 6 would loop past
    /// 5 and return 0. Conversely, a value of -1 would loop below 0,
    /// and return 5. Note that the min and max values are inclusive.
    /// </summary>
    public static int CycleThroughRange( int value, int min, int max )
    {
        // Min and Max are INCLUSIVE. So a range of 2—4 comprises three values.
        return Modulo( value - min, max - min + 1 ) + min;
    }

    /// <summary>
    /// <para>Calculate the position within an arbitrary range that
    /// a value resides, were it to loop around endlessly.</para>
    /// <para>For example, in a range of 0—5, a value of 6.5 would 
    /// loop past 5 and return 1.5. Conversely, a value of -1.2 would 
    /// loop below 0, and return 3.8.</para>
    /// Unlike the integer version of this method, the min and max values are NOT inclusive.
    /// </summary>
    public static float CycleThroughRange( float value, float min, float max )
    {
        return Modulo( value - min, max - min ) + min;
    }



    #region Remapping / Scaling to Range Methods


    /// <summary>
    /// Convert a [0 – 1] scalar to the [0 – 0.5] range.
    /// Super simple—it just halves the value and clamps the result.
    /// </summary>
    public static float ScaleToZeroAndHalf( float value )
    {
        return Mathf.Clamp( value * 0.5f, 0.0f, 0.5f );
    }

    /// <summary>
    /// Convert a [0 – 1] scalar to the [0.5 – 1] range.
    /// </summary>
    public static float ScaleToHalfAndOne( float value )
    {
        return Mathf.Clamp( value * 0.5f + 0.5f, 0.5f, 1.0f );
    }


    /// <summary>
    /// Returns a value, normalized from 0 to 1, representing the value's position within the min/max range provided.
    /// </summary>
    public static float ScaleToRange( float value, float minimum, float maximum )
    {
        return (value - minimum) / (maximum - minimum);
    }


    // Following function is from http://stackoverflow.com/questions/929103/convert-a-number-range-to-another-range-maintaining-ratio
    /// <summary>
    /// Given a value within a range, scale that value to a new range.
    /// For instance, a value of 5 between 0 and 10, scaled to the new range of 30 – 80, would become 55.
    /// </summary>
    /// <param name="value">The value, specified within the Original range, to be scaled.</param>
    public static float ScaleToNewRange( float value, float originalMin, float originalMax, float newMin, float newMax )
    {
        // First, protect against division by zero
        if( originalMin == originalMax || newMin == newMax ) { return newMin; }

        return ScaleToNewRange(
            value,
            originalMin,
            originalMax,
            newMin,
            newMax,
            false,
            false
        );
    }


    /// <summary>
    /// Given a value within a range, scale that value to a new range.
    /// For instance, a value of 5 between 0 and 10, scaled to the new range of 30 – 80, would become 55.
    /// This method also auto-corrects ranges that are defined in reverse order — 
    /// a min of 10 and a max of 5, for instance. These two values would be swapped.
    /// </summary>
    /// <param name="value">The value, specified within the Original range, to be scaled.</param>
    public static float ScaleToNewRangeWithSmartReversal( float value, float originalMin, float originalMax, float newMin, float newMax )
    {
        // First, protect against division by zero
        if( originalMin == originalMax || newMin == newMax ) { return newMin; }

        return ScaleToNewRange(
            value,
            originalMin,
            originalMax,
            newMin,
            newMax,
            originalMin != Mathf.Min( originalMin, originalMax ),
            newMin != Mathf.Min( newMin, newMax )
        );
    }


    /// <summary>
    /// Private method that does most of the Scale To New Range work.
    /// This exists separately so that any range reversals need only be calculated once.
    /// </summary>
    /// <param name="value">The value, specified within the Original range, to be scaled.</param>
    /// <param name="reverseOriginal">Should the Original Min and Original Max be swapped?</param>
    /// <param name="reverseNew">Should the New Min and New Max be swapped?</param>
    private static float ScaleToNewRange( float value, float originalMin, float originalMax, float newMin, float newMax, bool reverseOriginal, bool reverseNew )
    {
        float output;
        if( !reverseOriginal )
        {
            // The standard formula
            output = ((value - originalMin) * (newMax - newMin)) / (originalMax - originalMin);
        }
        else
        {
            // Original range values were reversed
            output = ((originalMax - value) * (newMax - newMin)) / (originalMax - originalMin);
        }
        if( !reverseNew )
        {
            output += newMin;       // The standard formula
        }
        else
        {
            output = newMax - output;   // New range is reversed
        }
        return output;

        // This standard formula could be more legibly read as...
        // (( value - originalMin ) * newRange / oldRange ) + newMin
    }

    #endregion


    // From Stack Overflow answer http://stackoverflow.com/a/1082938
    /// <summary>
    /// <para>Return the remainder after division of x by y.</para>
    /// This differs from the built-in % operator in that it supports negative dividend (x) values.
    /// </summary>
    /// <param name="x">The dividend value.</param>
    /// <param name="y">The divisor value.</param>
    public static int Modulo( int x, int y )
    {
        return ((x %= y) < 0) ? x + y : x;
    }

    /// <summary>
    /// <para>Return the remainder after division of x by y.</para>
    /// This differs from the built-in % operator in that it supports negative dividend (x) values.
    /// </summary>
    /// <param name="x">The dividend value.</param>
    /// <param name="y">The divisor value.</param>
    public static float Modulo( float x, float y )
    {
        return ((x %= y) < 0) ? x + y : x;
    }


    /// <summary>
    /// <para>Returns the angle between <paramref name="from"/> and <paramref name="to"/>, in radians.</para>
    /// Unity's Vector3.Angle() automatically converts to degrees. This method drops that extra operation.
    /// </summary>
    /// <param name="from">The angle extends round from this vector.</param>
    /// <param name="to">The angle extends round to this vector.</param>
    public static float AngleRadians( Vector3 from, Vector3 to )
    {
        return Mathf.Acos( Mathf.Clamp( Vector3.Dot( from.normalized, to.normalized ), -1f, 1f ) );
    }


}