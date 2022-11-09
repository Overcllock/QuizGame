public static class StringExtensions
{
    public static string SlashSafe(this string source)
    {
        return source.Replace("\\", "/");
    }

    public static string ToCamelCase(this string source)
    {
        if (!string.IsNullOrEmpty(source) && source.Length > 1)
        {
            return char.ToLowerInvariant(source[0]) + source.Substring(1);
        }

        return source;
    }

    public static bool IsNullOrEmpty(this string source)
    {
        return string.IsNullOrEmpty(source);
    }

    public static bool IsNullOrWhiteSpace(this string source)
    {
        return string.IsNullOrWhiteSpace(source);
    }

    public static string FirstCharToUpper(this string s)
    {
        if (string.IsNullOrEmpty(s)) return null;

        char[] chars = s.ToCharArray();
        chars[0] = char.ToUpper(chars[0]);
        return new string(chars);
    }
}