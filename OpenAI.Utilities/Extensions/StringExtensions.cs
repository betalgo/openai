using System.Text.RegularExpressions;

namespace OpenAI.Utilities;

public static partial class StringExtensions
{
    private static readonly Regex NewlineToSpaceRegex = NewlineToSpace();
    private static readonly Regex MultipleSpacesToSingleSpaceRegex = MultipleSpacesToSingleSpace();

    public static string RemoveNewlines(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        input = NewlineToSpaceRegex.Replace(input, " ");
        input = MultipleSpacesToSingleSpaceRegex.Replace(input, " ");
        return input;
    }

    [GeneratedRegex("\\r?\\n")]
    private static partial Regex NewlineToSpace();

    [GeneratedRegex(" {2,}")]
    private static partial Regex MultipleSpacesToSingleSpace();
}
