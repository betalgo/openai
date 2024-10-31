using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Playground.ExtensionsAndHelpers;

public static class ConsoleExtensions
{
    public static void WriteLine(string? value, ConsoleColor color = ConsoleColor.Gray)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ForegroundColor = defaultColor;
    }

    public static void WriteError(Error? error)
    {
        if (error == null)
        {
            throw new("Unknown Error");
        }

        WriteLine($"{error.Code}: {error.Message}", ConsoleColor.Red);
    }
}