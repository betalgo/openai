using OpenAI.GPT3.Tokenizer.GPT3;
using System.Text;

namespace OpenAI.Playground.TestHelpers;

internal static class TokenizerTestHelper
{
    public static async Task RunTokenizerTest()
    {
        ConsoleExtensions.WriteLine("Tokenizer Test is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Tokenizer Test:", ConsoleColor.DarkCyan);
            const string fileName = "TokenizerSample.txt";

            var input = await File.ReadAllTextAsync($"SampleData/{fileName}", Encoding.ASCII);
            var encodedList = TokenizerGpt3.Encode(input);
            if (encodedList.Count == 64)
            {
                ConsoleExtensions.WriteLine("Tokenizer Test Success", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("Tokenizer Test Failed", ConsoleColor.Red);
                ConsoleExtensions.WriteLine("Expected Token: 64 ", ConsoleColor.Red);
                ConsoleExtensions.WriteLine($"Found Token={encodedList.Count}", ConsoleColor.Red);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}