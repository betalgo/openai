using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class VisionTestHelper
{
    public static async Task RunSimpleVisionTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Vision Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Vision Test:", ConsoleColor.DarkCyan);

            var completionResult = await sdk.ChatCompletion.CreateCompletion(new()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are an image analyzer assistant."),
                    ChatMessage.FromUser(new List<MessageContent>
                    {
                        MessageContent.TextContent("What is on the picture in details?"),
                        MessageContent.ImageUrlContent("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg",
                            StaticValues.ImageStatics.ImageDetailTypes.High)
                    })
                },
                MaxTokens = 300,
                Model = Models.Gpt_4_vision_preview,
                N = 1
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

    public static async Task RunSimpleVisionStreamTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Vision Stream Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Vision Stream Test:", ConsoleColor.DarkCyan);

            var completionResult = sdk.ChatCompletion.CreateCompletionAsStream(new()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are an image analyzer assistant."),
                    ChatMessage.FromUser(new List<MessageContent>
                    {
                        MessageContent.TextContent("What’s in this image?"),
                        MessageContent.ImageUrlContent("https://upload.wikimedia.org/wikipedia/commons/thumb/d/dd/Gfp-wisconsin-madison-the-nature-boardwalk.jpg/2560px-Gfp-wisconsin-madison-the-nature-boardwalk.jpg",
                            StaticValues.ImageStatics.ImageDetailTypes.Low)
                    })
                },
                MaxTokens = 300,
                Model = Models.Gpt_4_vision_preview,
                N = 1
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

    public static async Task RunSimpleVisionTestUsingBase64EncodedImage(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Vision Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Vision with base64 encoded image Test:", ConsoleColor.DarkCyan);

            const string originalFileName = "image_edit_original.png";
            var originalFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{originalFileName}");

            var completionResult = await sdk.ChatCompletion.CreateCompletion(new()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are an image analyzer assistant."),
                    ChatMessage.FromUser(new List<MessageContent>
                    {
                        MessageContent.TextContent("What is on the picture in details?"),
                        MessageContent.ImageBinaryContent(originalFile, StaticValues.ImageStatics.ImageFileTypes.Png, StaticValues.ImageStatics.ImageDetailTypes.High)
                    })
                },
                MaxTokens = 300,
                Model = Models.Gpt_4_vision_preview,
                N = 1
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
}