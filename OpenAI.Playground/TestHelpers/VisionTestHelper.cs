using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers;

internal static class VisionTestHelper
{
    public static async Task RunSimpleVisionTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("VIsion Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Vision Test:", ConsoleColor.DarkCyan);
            var completionResult = await sdk.ChatCompletion.CreateVision(new VisionCreateRequest
            {
                Messages = new List<VisionChatMessage>
                {
                    VisionChatMessage.FromSystem(new VisionContent("text", "You are an image analyzer assistant")),
                    VisionChatMessage.FromUser(new List<VisionContent>
                        {
                            new("text", "What is on the picture in details?"), 
                            new("image_url", new VisionImageUrl("https://www.digitaltrends.com/wp-content/uploads/2016/06/1024px-Bill_Cunningham_at_Fashion_Week_photographed_by_Jiyang_Chen.jpg?p=1", "high"))
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

    public static async Task RunSimpleVisionStreamTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Vision Stream Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Vision Stream Test:", ConsoleColor.DarkCyan);

            var completionResult = sdk.ChatCompletion.CreateVisionAsStream(new VisionCreateRequest
            {
                Messages = new List<VisionChatMessage>
                {
                    VisionChatMessage.FromSystem(new VisionContent("text", "You are an image analyzer assistant")),
                    VisionChatMessage.FromUser(new List<VisionContent>
                        {
                            new("text", "What is on the picture in details?"), 
                            new("image_url", new VisionImageUrl("https://www.digitaltrends.com/wp-content/uploads/2016/06/1024px-Bill_Cunningham_at_Fashion_Week_photographed_by_Jiyang_Chen.jpg?p=1"))
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

    
}