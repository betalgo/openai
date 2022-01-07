using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models;
using OpenAI.GPT3.Models.RequestModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class CompletionTestHelper
    {
        public static async Task RunSimpleCompletionTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Fetching Engine List", ConsoleColor.DarkCyan);
                var completionResult = await sdk.Completions.Create(new CompletionCreateRequest()
                {
                    Prompt = "Once upon a time",
                    MaxTokens = 5
                }, Engines.Engine.Davinci);

                if (completionResult.Successful)
                {
                    Console.WriteLine(completionResult.Choices.FirstOrDefault());
                }
                else
                {
                    if (completionResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }
                    Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
                }
                Console.WriteLine(completionResult.Choices.FirstOrDefault());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}