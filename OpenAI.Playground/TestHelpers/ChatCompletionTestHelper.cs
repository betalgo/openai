using OpenAI.Builders;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class ChatCompletionTestHelper
{
    public static async Task RunSimpleChatCompletionTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Completion Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Chat Completion Test:", ConsoleColor.DarkCyan);
            var completionResult = await sdk.ChatCompletion.CreateCompletion(new()
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
                    throw new("Unknown Error");
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
            var completionResult = sdk.ChatCompletion.CreateCompletionAsStream(new()
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
                        throw new("Unknown Error");
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

    public static async Task RunSimpleCompletionStreamWithUsageTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Completion Stream Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Chat Completion Stream Test:", ConsoleColor.DarkCyan);
            var completionResult = sdk.ChatCompletion.CreateCompletionAsStream(new()
            {
                Messages = new List<ChatMessage>
                {
                    new(StaticValues.ChatMessageRoles.System, "You are a helpful assistant."),
                    new(StaticValues.ChatMessageRoles.User, "Who won the world series in 2020?"),
                    new(StaticValues.ChatMessageRoles.System, "The Los Angeles Dodgers won the World Series in 2020."),
                    new(StaticValues.ChatMessageRoles.User, "Tell me a story about The Los Angeles Dodgers")
                },
                StreamOptions = new()
                {
                    IncludeUsage = true
                },
                MaxTokens = 150,
                Model = Models.Gpt_3_5_Turbo
            });

            await foreach (var completion in completionResult)
            {
                if (completion.Successful)
                {
                    if (completion.Usage != null)
                    {
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine($"Usage: {completion.Usage.TotalTokens}");
                    }
                    else
                    {
                        Console.Write(completion.Choices.First().Message.Content);
                    }
                }
                else
                {
                    if (completion.Error == null)
                    {
                        throw new("Unknown Error");
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
        ConsoleExtensions.WriteLine("Chat Tool Functions Call Testing is starting:", ConsoleColor.Cyan);

        // example taken from:
        // https://github.com/openai/openai-cookbook/blob/main/examples/How_to_call_functions_with_chat_models.ipynb

        var fn1 = new FunctionDefinitionBuilder("get_current_weather", "Get the current weather").AddParameter("location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA"))
            .AddParameter("format", PropertyDefinition.DefineEnum(new() { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .Validate()
            .Build();

        var fn2 = new FunctionDefinitionBuilder("get_n_day_weather_forecast", "Get an N-day weather forecast").AddParameter("location", new() { Type = "string", Description = "The city and state, e.g. San Francisco, CA" })
            .AddParameter("format", PropertyDefinition.DefineEnum(new() { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .AddParameter("num_days", PropertyDefinition.DefineInteger("The number of days to forecast"))
            .Validate()
            .Build();
        var fn3 = new FunctionDefinitionBuilder("get_current_datetime", "Get the current date and time, e.g. 'Saturday, June 24, 2023 6:14:14 PM'").Build();

        var fn4 = new FunctionDefinitionBuilder("identify_number_sequence", "Get a sequence of numbers present in the user message")
            .AddParameter("values", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Sequence of numbers specified by the user")))
            .Build();
        try
        {
            ConsoleExtensions.WriteLine("Chat Function Call Test:", ConsoleColor.DarkCyan);

            var request = new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous."),
                    ChatMessage.FromUser("Give me a weather report for Chicago, USA, for the next 5 days.")
                },
                Tools = new List<ToolDefinition> { ToolDefinition.DefineFunction(fn1), ToolDefinition.DefineFunction(fn2), ToolDefinition.DefineFunction(fn3), ToolDefinition.DefineFunction(fn4) },
                // optionally, to force a specific function:
                //ToolChoice = ToolChoice.FunctionChoice("get_current_weather"),
                // or auto tool choice:
                //ToolChoice = ToolChoice.Auto,
                MaxTokens = 50,
                Model = Models.Gpt_3_5_Turbo
            };

            var completionResult = await sdk.ChatCompletion.CreateCompletion(request);

            /*  expected output along the lines of:

                Message:
                Function call:  get_n_day_weather_forecast
                  location: Chicago, USA
                  format: celsius
                  num_days: 5
            */


            if (completionResult.Successful)
            {
                var choice = completionResult.Choices.First();
                Console.WriteLine($"Message:        {choice.Message.Content}");

                var tools = choice.Message.ToolCalls;
                if (tools != null)
                {
                    request.Messages.Add(choice.Message);

                    Console.WriteLine($"Tools: {tools.Count}");
                    foreach (var toolCall in tools)
                    {
                        Console.WriteLine($"  {toolCall.Id}: {toolCall.FunctionCall}");

                        var fn = toolCall.FunctionCall;
                        if (fn != null)
                        {
                            Console.WriteLine($"  Function call:  {fn.Name}");
                            foreach (var entry in fn.ParseArguments())
                            {
                                Console.WriteLine($"    {entry.Key}: {entry.Value}");
                            }

                            if (fn.Name == "get_n_day_weather_forecast")
                            {
                                request.Messages.Add(ChatMessage.FromTool("10 Degrees", toolCall.Id!));
                            }
                        }
                    }
                }
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new("Unknown Error");
                }

                Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
            }

            var completionResultAfterTool = await sdk.ChatCompletion.CreateCompletion(request);

            if (completionResultAfterTool.Successful)
            {
                Console.WriteLine(completionResultAfterTool.Choices.First().Message.Content);
            }
            else
            {
                if (completionResultAfterTool.Error == null)
                {
                    throw new("Unknown Error");
                }

                Console.WriteLine($"{completionResultAfterTool.Error.Code}: {completionResultAfterTool.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task RunChatFunctionCallTestAsStream(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Tool Functions Call Stream Testing is starting:", ConsoleColor.Cyan);

        // example taken from:
        // https://github.com/openai/openai-cookbook/blob/main/examples/How_to_call_functions_with_chat_models.ipynb

        var fn1 = new FunctionDefinitionBuilder("get_current_weather", "Get the current weather").AddParameter("location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA"))
            .AddParameter("format", PropertyDefinition.DefineEnum(new() { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .Validate()
            .Build();

        var fn2 = new FunctionDefinitionBuilder("get_n_day_weather_forecast", "Get an N-day weather forecast").AddParameter("location", new() { Type = "string", Description = "The city and state, e.g. San Francisco, CA" })
            .AddParameter("format", PropertyDefinition.DefineEnum(new() { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .AddParameter("num_days", PropertyDefinition.DefineInteger("The number of days to forecast"))
            .Validate()
            .Build();

        var fn3 = new FunctionDefinitionBuilder("get_current_datetime", "Get the current date and time, e.g. 'Saturday, June 24, 2023 6:14:14 PM'").Build();

        var fn4 = new FunctionDefinitionBuilder("identify_number_sequence", "Get a sequence of numbers present in the user message")
            .AddParameter("values", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Sequence of numbers specified by the user")))
            .Build();
        var fn5 = new FunctionDefinitionBuilder("google_search", "Gets a result from Google Search").AddParameter("search_term", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Search Term"))).Build();
        var fn6 = new FunctionDefinitionBuilder("getURL", "Downloads the content of given website").AddParameter("URL", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Search Term"))).Build();

        try
        {
            ConsoleExtensions.WriteLine("Chat Function Call Test:", ConsoleColor.DarkCyan);

            var request = new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a bot that performs internet searches and also downloads content from websites."),
                    // to test weather forecast functions:
                    ChatMessage.FromUser("I need you to first search Google for \"Cat\" and, at the same time, download the contents from https://www.wired.com.")
                    //ChatMessage.FromUser("Give me a weather report for Chicago, USA, for the next 5 days and also current weather.")
                    // or to test array functions, use this instead:
                    // ChatMessage.FromUser("And also The combination is: One. Two. Three. Four. Five."),
                },
                Tools = new List<ToolDefinition>
                    { ToolDefinition.DefineFunction(fn1), ToolDefinition.DefineFunction(fn2), ToolDefinition.DefineFunction(fn3), ToolDefinition.DefineFunction(fn4), ToolDefinition.DefineFunction(fn5), ToolDefinition.DefineFunction(fn6) },
                // optionally, to force a specific function:
                //ToolChoice = ToolChoice.FunctionChoice("get_current_weather"),
                // or auto tool choice:
                ToolChoice = ToolChoice.Auto,
                //MaxTokens = 50,
                Model = Models.Gpt_4_1106_preview
            };

            var completionResults = sdk.ChatCompletion.CreateCompletionAsStream(request);

            /*  when testing weather forecasts, expected output should be along the lines of:

                Message:
                Function call:  get_n_day_weather_forecast
                  location: Chicago, USA
                  format: celsius
                  num_days: 5
            */

            /*  when testing array functions, expected output should be along the lines of:

                Message:
                Function call:  identify_number_sequence
                  values: [1, 2, 3, 4, 5]
            */
            var functionArguments = new Dictionary<int, string>();
            await foreach (var completionResult in completionResults)
            {
                if (completionResult.Successful)
                {
                    var choice = completionResult.Choices.First();
                    Console.WriteLine($"Message:        {choice.Message.Content}");

                    var tools = choice.Message.ToolCalls;
                    if (tools != null)
                    {
                        request.Messages.Add(choice.Message);

                        Console.WriteLine($"Tools: {tools.Count}");
                        for (var i = 0; i < tools.Count; i++)
                        {
                            var toolCall = tools[i];
                            Console.WriteLine($"  {toolCall.Id}: {toolCall.FunctionCall}");

                            var fn = toolCall.FunctionCall;
                            if (fn != null)
                            {
                                if (!string.IsNullOrEmpty(fn.Name))
                                {
                                    Console.WriteLine($"  Function call:  {fn.Name}");
                                }

                                if (!string.IsNullOrEmpty(fn.Arguments))
                                {
                                    if (functionArguments.TryGetValue(i, out var currentArguments))
                                    {
                                        currentArguments += fn.Arguments;
                                    }
                                    else
                                    {
                                        currentArguments = fn.Arguments;
                                    }

                                    functionArguments[i] = currentArguments;
                                    fn.Arguments = currentArguments;

                                    try
                                    {
                                        foreach (var entry in fn.ParseArguments())
                                        {
                                            Console.WriteLine($"    {entry.Key}: {entry.Value}");
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        // ignore
                                    }
                                }

                                if (fn.Name == "google_search")
                                {
                                    request.Messages.Add(ChatMessage.FromTool("Tom", toolCall.Id!));
                                }

                                if (fn.Name == "getURL")
                                {
                                    request.Messages.Add(ChatMessage.FromTool("News", toolCall.Id!));
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (completionResult.Error == null)
                    {
                        throw new("Unknown Error");
                    }

                    Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
                }
            }

            var completionResultsAfterTool = sdk.ChatCompletion.CreateCompletionAsStream(request);

            await foreach (var completion in completionResultsAfterTool)
            {
                if (completion.Successful)
                {
                    Console.Write(completion.Choices.First().Message.Content);
                }
                else
                {
                    if (completion.Error == null)
                    {
                        throw new("Unknown Error");
                    }

                    Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    //https://platform.openai.com/docs/guides/structured-outputs/how-to-use
    public static async Task RunChatWithJsonSchemaResponseFormat(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Completion Testing is starting:", ConsoleColor.Cyan);
        try
        {
            var completionResult = await sdk.ChatCompletion.CreateCompletion(new()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a helpful math tutor. Guide the user through the solution step by step."),
                    ChatMessage.FromUser("how can I solve 8x + 7 = -23")
                },
                Model = "gpt-4o-2024-08-06",
                ResponseFormat = new()
                {
                    Type = StaticValues.CompletionStatics.ResponseFormat.JsonSchema,
                    JsonSchema = new()
                    {
                        Name = "math_response",
                        Strict = true,
                        Schema = PropertyDefinition.DefineObject(new Dictionary<string, PropertyDefinition>
                        {
                            {
                                "steps", PropertyDefinition.DefineArray(PropertyDefinition.DefineObject(new Dictionary<string, PropertyDefinition>
                                {
                                    { "explanation", PropertyDefinition.DefineString("The explanation of the step") },
                                    { "output", PropertyDefinition.DefineString("The output of the step") }
                                }, new List<string> { "explanation", "output" }, false, "A step in the mathematical process", null))
                            },
                            {
                                "final_answer", PropertyDefinition.DefineString("The final answer of the mathematical process")
                            }
                        }, new List<string> { "steps", "final_answer" }, false, "Response containing steps and final answer of a mathematical process", null)
                    }
                }
            });

            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.First().Message.Content);
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new("Unknown Error");
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

    public static async Task RunChatReasoningModel(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Chat Completion Reasoning Testing is starting:", ConsoleColor.Cyan);
        try
        {
            var completionResult = await sdk.ChatCompletion.CreateCompletion(new()
            {
                Messages = [
                    ChatMessage.FromUser("Write a bash script that takes a matrix represented as a string with format '[1,2],[3,4],[5,6]' and prints the transpose in the same format.")
                ],
                MaxCompletionTokens = 2000,
                Model = Models.O1_preview
            });

            if (completionResult.Successful)
            {
                Console.WriteLine(completionResult.Choices.First().Message.Content);
                Console.WriteLine("Used:" + (completionResult.Usage.CompletionTokensDetails?.ReasoningTokens??0) + " reasoning tokens" );
            }
            else
            {
                if (completionResult.Error == null)
                {
                    throw new("Unknown Error");
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
}