using UnityEngine;
using System;

/// <summary>
/// Contains extension methods for working with enum/int Flags, as well 
/// as some helper methods, primarily for evaluating and debugging Flags.
/// </summary>
public static class FlagsHelper
{

    #region Int Flags

    // Originally passed bitfield with "this" and called it from an instance of int
    // but that wasn't working correctly. Changing it to "ref" fixed it.

    /// <summary>Add the specified flag to this bitfield.</summary>
    public static void AddFlag( ref int bitfield, int flag )
    {
        // | is an inclusive OR operator. If either bit is 1, the resulting bit is 1.
        // << bitshifts 1 (in binary, 0001) to the left according to the flag
        bitfield |= 1 << flag;
    }


    /// <summary>Add the specified flag to this bitfield.</summary>
    public static void RemoveFlag( ref int bitfield, int flag )
    {
        // & sets a bit to 1 only if BOTH operands have 1.
        // The squiggle, ~, inverts the second operand.
        // << bitshifts 1 (in binary, 0001) to the left according to the flag
        bitfield &= ~(1 << flag);
    }


    /// <summary>Test that the specified flag is contained in this bitfield.</summary>
    /// <returns>True if the bitfield contains the flag, False if not.</returns>
    public static bool HasFlag( this int bitfield, int flag )
    {
        // & sets a bit to 1 only if BOTH operands have 1.
        // So this equation is saying, if you strip everything away from bitfield
        // except flag, and you end up with flag, then bitfield has that flag.
        return (bitfield & flag) == flag;
    }

    #endregion

    #region Layer Flags


    /// <summary>Add the specified layer to this LayerMask.</summary>
    public static void AddLayer( this LayerMask mask, int layerToAdd )
    {
        mask |= (1 << layerToAdd);
    }


    /// <summary>Add the specified layer to this LayerMask.</summary>
    public static void RemoveFlag( this LayerMask mask, int layerToRemove )
    {
        mask &= ~(1 << layerToRemove);
    }


    /// <summary>Test that the specified layer is contained in this LayerMask.</summary>
    /// <returns>True if the LayerMask contains the layer, False if not.</returns>
    public static bool HasFlag( this LayerMask mask, int layerToTest )
    {
        var bit = 1 << layerToTest;
        return (mask & bit) == bit;
    }

    #endregion

    #region Int Flags (Non-Reference)


    /// <summary>Add the specified flag to this bitfield.</summary>
    public static int AddFlag( int bitfield, int flag )
    {
        return bitfield |= (1 << flag);
    }


    /// <summary>Add the specified flag to this bitfield.</summary>
    public static int RemoveFlag( int bitfield, int flag )
    {
        return bitfield &= ~(1 << flag);
    }

    #endregion


    /// <summary>Count the number of flags set in the specified bitfield.</summary>
    private static int Count( int value )
    {
        int iCount = 0;

        //Loop so long as there are still bits
        while( value != 0 )
        {
            //Remove the end bit
            value = value & (value - 1);

            //Increment the count
            iCount++;
        }

        return iCount;
    }

    /// <summary>
    /// Assemble a string that writes out each flag enum comprising the target enum (which must be cast as an integer).
    /// </summary>
    private static string ToString<T>( int value )
    {
        string result = default( string );

        //Loop the value while there are still bits
        while( value != 0 )
        {
            result += Enum.Format( typeof( T ), value & ~value + 1, "G" );

            //Remove the end bit
            value &= value - 1;

            if( value != 0 )
                result += ", ";
        }
        return result;
    }

    /// <summary>
    /// Assemble a string that writes out each bit comprising this integer.
    /// For example, the value 72 would produce "1001000", prefaced with a bunch of 0s.
    /// </summary>
    public static string ToBinaryString( this int value )
    {
        char[] bits = new char[32];
        int pos = 31;
        int i = 0;

        while( i < 32 )
        {
            if( (value & (1 << i)) != 0 )
            {
                bits[pos] = '1';
            }
            else
            {
                bits[pos] = '0';
            }
            pos--;
            i++;
        }
        return new string( bits );
    }

}