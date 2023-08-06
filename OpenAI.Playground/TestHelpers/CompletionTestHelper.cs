using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers;

internal static class CompletionTestHelper
{
    public static async Task RunSimpleCompletionTest(IOpenAIService sdk, CancellationToken cancellationToken = default)
    {
        ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Completion Test:", ConsoleColor.DarkCyan);
            var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest
            {
                Prompt = "Once upon a time",
                //    PromptAsList = new []{"Once upon a time"},
                MaxTokens = 5,
                LogProbs = 1
            }, Models.Davinci, cancellationToken);

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

    public static async Task RunSimpleCompletionTestWithCancellationToken(IOpenAIService sdk)
    {
        var cancellationToken = new CancellationTokenSource(TimeSpan.Zero).Token;
        try
        {
            await RunSimpleCompletionTest(sdk, cancellationToken);
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("Completion test has been cancelled.");
        }
    }

    public static async Task RunSimpleCompletionTest2(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Completion Test:", ConsoleColor.DarkCyan);
            var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest
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
            var completionResult = await sdk.Completions.CreateCompletion(new CompletionCreateRequest
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

    public static async Task RunSimpleCompletionStreamTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Completion Stream Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Completion Stream Test:", ConsoleColor.DarkCyan);
            var completionResult = sdk.Completions.CreateCompletionAsStream(new CompletionCreateRequest
            {
                Prompt = "Once upon a time",
                MaxTokens = 500
            }, Models.Davinci);

            await foreach (var completion in completionResult)
            {
                if (completion.Successful)
                {
                    Console.Write(completion.Choices.FirstOrDefault()?.Text);
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunSimpleCompletionStreamTestWithCancellationToken(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Completion Stream Testing is starting:", ConsoleColor.Cyan);
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(2)).Token;
        cancellationToken.Register(() => Console.WriteLine("Cancellation Token has been cancelled."));

        try
        {
            ConsoleExtensions.WriteLine("Completion Stream Test:", ConsoleColor.DarkCyan);
            var completionResult = sdk.Completions.CreateCompletionAsStream(new CompletionCreateRequest
            {
                Prompt = "Once upon a time",
                MaxTokens = 100
            }, Models.Davinci, cancellationToken);

            try
            {
                await foreach (var completion in completionResult.WithCancellation(cancellationToken))
                {
                    if (completion.Successful)
                    {
                        Console.Write(completion.Choices.FirstOrDefault()?.Text);
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
            }
            catch (OperationCanceledException)
            {
                ConsoleExtensions.WriteLine("Operation Cancelled", ConsoleColor.Green);
            }

            Console.WriteLine("");
            Console.WriteLine("Complete");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}