using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class CompletionTestHelper
    {
        public static async Task RunSimpleCompletionTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Completion Test:", ConsoleColor.DarkCyan);
                var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest()
                {
                    Prompt = "Once upon a time",
                    //    PromptAsList = new []{"Once upon a time"},
                    MaxTokens = 5,
                    LogProbs = 1
                }, Models.Davinci);

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

        public static async Task RunSimpleCompletionTest2(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Completion Test:", ConsoleColor.DarkCyan);
                var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest()
                {
                    Prompt = "Once upon a time",
                    //    PromptAsList = new []{"Once upon a time"},
                    MaxTokens = 5,
                    LogProbs = 1,
                    Model = Models.TextDavinciV3
                });

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

        public static async Task RunSimpleCompletionTest3(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Completion Test:", ConsoleColor.DarkCyan);
                //Parameter Model should override the Model in the ObjectModel
                var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest()
                {
                    Prompt = "Once upon a time",
                    //    PromptAsList = new []{"Once upon a time"},
                    MaxTokens = 5,
                    LogProbs = 1,
                    Model = Models.Ada
                }, Models.TextDavinciV3);

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

        public static async Task<string> RunSimpleCompletionStreamTest(IOpenAIService sdk, string prompt = "Once upon a time")
        {
            ConsoleExtensions.WriteLine("Completion Stream Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Completion Stream Test:", ConsoleColor.DarkCyan);

                var completionCreateRequest = new CompletionCreateRequest()
                {
                    Prompt = prompt,
                    MaxTokens = 50
                };

                var completionResult = sdk.Completions.CreateCompletionAsStream(completionCreateRequest, Models.Davinci);

                Console.Write($"[{prompt}] ");

                var fullCompletionResult = prompt;
                await foreach (var completion in completionResult)
                {
                    if (completion.Successful)
                    {
                        var completionText = completion.Choices.FirstOrDefault()?.Text;
                        Console.Write(completionText);
                        fullCompletionResult += completionText;
                    }
                    else
                    {
                        if (completion.Error == null)
                        {
                            throw new Exception("Unknown Error");
                        }

                        Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
                    }
                }

                Console.WriteLine("");
                Console.WriteLine("Complete");

                return fullCompletionResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}