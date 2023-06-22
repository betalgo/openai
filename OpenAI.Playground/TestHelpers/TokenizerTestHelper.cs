using OpenAI.Tokenizer.GPT3;

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

            var input = await FileExtensions.ReadAllTextAsync($"SampleData/{fileName}");
            var encodedList = TokenizerGpt3.Encode(input).ToList();
            if (encodedList.Count == 68)
            {
                ConsoleExtensions.WriteLine("Tokenizer Test Success", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("Tokenizer Test Failed", ConsoleColor.Red);
                ConsoleExtensions.WriteLine("Expected Token: 68 ", ConsoleColor.Red);
                ConsoleExtensions.WriteLine($"Found Token={encodedList.Count}", ConsoleColor.Red);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunTokenizerCountTest()
    {
        ConsoleExtensions.WriteLine("Tokenizer Count Test is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Tokenizer Test:", ConsoleColor.DarkCyan);
            const string fileName = "TokenizerSample.txt";

            var input = await FileExtensions.ReadAllTextAsync($"SampleData/{fileName}");
            var encodedList = TokenizerGpt3.TokenCount(input);
            if (encodedList == 68)
            {
                ConsoleExtensions.WriteLine("Tokenizer Test Success", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("Tokenizer Test Failed", ConsoleColor.Red);
                ConsoleExtensions.WriteLine("Expected Token: 68 ", ConsoleColor.Red);
                ConsoleExtensions.WriteLine($"Found Token={encodedList}", ConsoleColor.Red);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunTokenizerTestCrClean()
    {
        ConsoleExtensions.WriteLine("Tokenizer Test is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Tokenizer Test:", ConsoleColor.DarkCyan);
            const string fileName = "TokenizerSample.txt";

            var input = await FileExtensions.ReadAllTextAsync($"SampleData/{fileName}");
            var encodedList = TokenizerGpt3.Encode(input, true).ToList();
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