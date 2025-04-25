namespace SG.Shared.Extensions;

public static class StringExtensions
{
    public static string RemoveLastChar(this string value, char? character = null)
    {
        if( string.IsNullOrEmpty(value))
        {
            return value;
        }

        if(character is not null)
        {   
            char lastCharacter = value[^1];
            return character.Equals(lastCharacter) ? value[..^1] : value;
        }
       
        return value[..^1];
    }
}
