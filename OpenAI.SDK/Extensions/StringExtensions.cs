namespace OpenAI.Extensions;

/// <summary>
///     Extension methods for string manipulation
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Remove the search string from the beginning of string if it exists
    /// </summary>
    /// <param name="text"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public static string RemoveIfStartWith(this string text, string search)
    {
        var pos = text.IndexOf(search, StringComparison.Ordinal);
        return pos != 0 ? text : text.Substring(search.Length);
    }
}