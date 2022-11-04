using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class ImageTestHelper
    {
        public static async Task RunSimpleCreateImageTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Image Create Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Image Create Test:", ConsoleColor.DarkCyan);
                var imageResult = await sdk.Image.CreateImage(new ImageCreateRequest
                {
                    Prompt = "A cute baby sea otter",
                    N = 2,
                    Size = StaticValues.ImageStatics.Size.Size256,
                    ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
                    User = "TestUser"
                });


                if (imageResult.Successful)
                {
                    Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
                }
                else
                {
                    if (imageResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }

                    Console.WriteLine($"{imageResult.Error.Code}: {imageResult.Error.Message}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}