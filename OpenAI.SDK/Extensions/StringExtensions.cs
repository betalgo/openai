namespace OpenAI.GPT3.Extensions;

/// <summary>
///     Extension methods for string manipulation
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Remove the search string from the begging of string if exist
    /// </summary>
    /// <param name="text"></param>
    /// <param name="search"></param>
    /// <returns></returns>
    public static string RemoveIfStartWith(this string text, string search)
    {
        var pos = text.IndexOf(search, StringComparison.Ordinal);
        return pos != 0 ? text : text[search.Length..];
    }

    /// <summary>
    ///     Throw ArgumentNullException if toCheck is null or empty
    /// </summary>
    /// <param name="toCheck"></param>
    /// <param name="name"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ThrowIfIsNullOrEmpty(this string? toCheck, string name)
    {
        if (string.IsNullOrEmpty(toCheck))
            throw new ArgumentNullException(name);
    }

    /// <summary>
    ///     Throw ArgumentNullException if toCheck is not null or empty
    /// </summary>
    /// <param name="toCheck"></param>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void ThrowIfNotNullOrEmpty(this string? toCheck, string name, string description)
    {
        if (!string.IsNullOrEmpty(toCheck))
            throw new ArgumentNullException($"{name} {description} {name}");
    }
}