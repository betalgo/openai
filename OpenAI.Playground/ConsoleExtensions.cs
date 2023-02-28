namespace OpenAI.Playground;

public static class ConsoleExtensions
{
    public static void WriteLine(string value, ConsoleColor color)
    {
        var defaultColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(value);
        Console.ForegroundColor = defaultColor;
    }
}