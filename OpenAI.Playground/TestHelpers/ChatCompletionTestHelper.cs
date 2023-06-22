using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers;

internal static class ChatCompletionTestHelper
{
    public static async Task RunSimpleChatCompletionTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Completion Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Chat Completion Test:", ConsoleColor.DarkCyan);
            var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a helpful assistant."),
                    ChatMessage.FromUser("Who won the world series in 2020?"),
                    ChatMessage.FromAssistant("The Los Angeles Dodgers won the World Series in 2020."),
                    ChatMessage.FromUser("Where was it played?")
                },
                MaxTokens = 50,
                Model = Models.Gpt_3_5_Turbo
            });

            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.First().Message.Content);
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunSimpleCompletionStreamTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Completion Stream Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Chat Completion Stream Test:", ConsoleColor.DarkCyan);
            var completionResult = sdk.ChatCompletion.CreateCompletionAsStream(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    new(StaticValues.ChatMessageRoles.System, "You are a helpful assistant."),
                    new(StaticValues.ChatMessageRoles.User, "Who won the world series in 2020?"),
                    new(StaticValues.ChatMessageRoles.System, "The Los Angeles Dodgers won the World Series in 2020."),
                    new(StaticValues.ChatMessageRoles.User, "Tell me a story about The Los Angeles Dodgers")
                },
                MaxTokens = 150,
                Model = Models.Gpt_3_5_Turbo
            });

            await foreach (var completion in completionResult)
            {
                if (completion.Successful)
                {
                    Console.Write(completion.Choices.First().Message.Content);
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

    public static async Task RunChatFunctionCallTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Function Call Testing is starting:", ConsoleColor.Cyan);

        // example taken from:
        // https://github.com/openai/openai-cookbook/blob/main/examples/How_to_call_functions_with_chat_models.ipynb

        var fn1 = new FunctionDefinitionBuilder("get_current_weather", "Get the current weather")
            .AddParameter("location", "string", "The city and state, e.g. San Francisco, CA")
            .AddParameter("format", "string", "The temperature unit to use. Infer this from the users location.",
                @enum: new List<string> { "celsius", "fahrenheit" })
            .Build();

        var fn2 = new FunctionDefinitionBuilder("get_n_day_weather_forecast", "Get an N-day weather forecast")
            .AddParameter("location", "string", "The city and state, e.g. San Francisco, CA")
            .AddParameter("format", "string", "The temperature unit to use. Infer this from the users location.",
                @enum: new List<string> { "celsius", "fahrenheit" })
            .AddParameter("num_days", "integer", "The number of days to forecast")
            .Build();

        try
        {
            ConsoleExtensions.WriteLine("Chat Function Call Test:", ConsoleColor.DarkCyan);
            var completionResults = sdk.ChatCompletion.CreateCompletionAsStream(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous."),
                    ChatMessage.FromUser("Give me a weather report for Chicago, USA, for the next 5 days."),
                },
                Functions = new List<FunctionDefinition> { fn1, fn2 },
                // optionally, to force a specific function:
                // FunctionCall = new Dictionary<string, string> { { "name", "get_current_weather" } },
                MaxTokens = 50,
                Model = Models.Gpt_3_5_Turbo_0613
            });

            /*  expected output along the lines of:
             
                Message:
                Function call:  get_n_day_weather_forecast
                  location: Chicago, USA
                  format: celsius
                  num_days: 5
            */

            await foreach (var completionResult in completionResults)
            {
                if (completionResult.Successful)
                {
                    var choice = completionResult.Choices.First();
                    Console.WriteLine($"Message:        {choice.Message.Content}");

                    var fn = choice.Message.FunctionCall;
                    if (fn != null)
                    {
                        Console.WriteLine($"Function call:  {fn.Name}");
                        foreach (var entry in fn.ParseArguments())
                        {
                            Console.WriteLine($"  {entry.Key}: {entry.Value}");
                        }
                    }
                }
                else
                {
                    if (completionResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                    Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}