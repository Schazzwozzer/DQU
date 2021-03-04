using System;
using System.Globalization;

public static class StringHelper
{
    /// <summary>
    /// Return true if the first character is an exclamation mark, question 
    /// mark, or a period followed by a separator (space, line break).
    /// </summary>
    public static bool IsEndOfSentence( char firstCharacter, char secondCharacter )
    {
        // This is a pretty simplistic way to handle this, and misses certain edgecases—
        // —initials, for instance. But it should work for most or all of our needs.
        return firstCharacter.Equals( '.' ) && Char.IsSeparator( secondCharacter ) ||
               firstCharacter.Equals( '!' ) ||
               firstCharacter.Equals( '?' );
    }

    /// <summary>Return true if the character is a space.</summary>
    public static bool IsSpace( char c )
    {
        return CharUnicodeInfo.GetUnicodeCategory( c ) == UnicodeCategory.SpaceSeparator;
    }

    /// <summary>
    /// Return true if the character represents a line break.
    /// </summary>
    public static bool IsLineBreak( char c )
    {
        return c.Equals( '\r' ) || c.Equals( '\n' );
    }

}