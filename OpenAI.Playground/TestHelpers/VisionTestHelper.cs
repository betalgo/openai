using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using static OpenAI.ObjectModels.StaticValues;

namespace OpenAI.Playground.TestHelpers;

internal static class VisionTestHelper
{
    public static async Task RunSimpleVisionTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("VIsion Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Vision Test:", ConsoleColor.DarkCyan);

            var completionResult = await sdk.ChatCompletion.CreateCompletion(
                new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are an image analyzer assistant."),
                        ChatMessage.FromVisionUser(
                            new List<VisionContent>
                            {
                                VisionContent.TextContent("What is on the picture in details?"),
                                VisionContent.ImageUrlContent(
                                    "https://www.digitaltrends.com/wp-content/uploads/2016/06/1024px-Bill_Cunningham_at_Fashion_Week_photographed_by_Jiyang_Chen.jpg?p=1",
                                    ImageStatics.ImageDetailTypes.High
                                )
                            }
                        ),
                    },
                    MaxTokens = 300,
                    Model = Models.Gpt_4_vision_preview,
                    N = 1
                }
            );

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

                Console.WriteLine(
                    $"{completionResult.Error.Code}: {completionResult.Error.Message}"
                );
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

            var completionResult = sdk.ChatCompletion.CreateCompletionAsStream(
                new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are an image analyzer assistant."),
                        ChatMessage.FromVisionUser(
                            new List<VisionContent>
                            {
                                VisionContent.TextContent("What is on this picture?"),
                                VisionContent.ImageUrlContent(
                                    "https://www.digitaltrends.com/wp-content/uploads/2016/06/1024px-Bill_Cunningham_at_Fashion_Week_photographed_by_Jiyang_Chen.jpg?p=1",
                                    ImageStatics.ImageDetailTypes.Low
                                )
                            }
                        ),
                    },
                    MaxTokens = 300,
                    Model = Models.Gpt_4_vision_preview,
                    N = 1
                }
            );

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

                    Console.WriteLine(
                        $"{completion.Error.Code}: {completion.Error.Message}"
                    );
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
            ConsoleExtensions.WriteLine(
                "Vision with base64 encoded image Test:",
                ConsoleColor.DarkCyan
            );

            const string originalFileName = "image_edit_original.png";
            var originalFile = await FileExtensions.ReadAllBytesAsync(
                $"SampleData/{originalFileName}"
            );

            var completionResult = await sdk.ChatCompletion.CreateCompletion(
                new ChatCompletionCreateRequest
                {
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem("You are an image analyzer assistant."),
                        ChatMessage.FromVisionUser(
                            new List<VisionContent>
                            {
                                VisionContent.TextContent("What is on the picture in details?"),
                                VisionContent.ImageBinaryContent(
                                    originalFile,
                                    ImageStatics.ImageFileTypes.Png,
                                    ImageStatics.ImageDetailTypes.High
                                )
                            }
                        ),
                    },
                    MaxTokens = 300,
                    Model = Models.Gpt_4_vision_preview,
                    N = 1
                }
            );

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

                Console.WriteLine(
                    $"{completionResult.Error.Code}: {completionResult.Error.Message}"
                );
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
