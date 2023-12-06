using System.Text.RegularExpressions;

namespace OpenAI.Utilities;

public static partial class StringExtensions
{
    private static readonly Regex NewlineToSpaceRegex = new Regex("\\r?\\n");
    private static readonly Regex MultipleSpacesToSingleSpaceRegex = new Regex(" {2,}");

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
}
