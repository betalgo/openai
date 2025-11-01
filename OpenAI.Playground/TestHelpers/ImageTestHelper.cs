using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Interfaces;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class ImageTestHelper
{
    public static async Task RunSimpleCreateImageTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Image Create Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Image Create Test:", ConsoleColor.DarkCyan);
            var imageResult = await sdk.Image.CreateImage(new()
            {
                Prompt = "Laser cat eyes",
                N = 1,
                Size = ImageSize.Size256
            });


            if (imageResult.Successful)
            {
                Console.WriteLine(string.Join("\n", imageResult.Data!.Select(r => r.Url!)));
            }
            else
            {
                if (imageResult.Error == null)
                {
                    throw new("Unknown Error");
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

    public static async Task RunSimpleCreateImageEditTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Image Edit Create Testing is starting:", ConsoleColor.Cyan);
        const string maskFileName = "image_edit_mask.png";
        const string originalFileName = "image_edit_original.png";

        // Images should be in png format with ARGB. I got help from this website to generate sample mask
        // https://www.online-image-editor.com/
        var originalFile = File.OpenRead($"SampleData/{originalFileName}");
        var maskFile = File.OpenRead($"SampleData/{maskFileName}");

        try
        {
            ConsoleExtensions.WriteLine("Image  Edit Create Test:", ConsoleColor.DarkCyan);
            var imageResult = await sdk.Image.CreateImageEdit(new()
            {
                Image = new FormFile(originalFileName, originalFile),
                Mask = new(maskFileName, maskFile),
                Prompt = "A sunlit indoor lounge area with a pool containing a cat",
                N = 4,
                Size = ImageSize.Size1024,
                ResponseFormat = ImageResponseFormat.Url,
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
                    throw new("Unknown Error");
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

    public static async Task RunSimpleCreateImageVariationTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Image Variation Create Testing is starting:", ConsoleColor.Cyan);
        const string originalFileName = "image_edit_original.png";
        var originalFile = File.OpenRead($"SampleData/{originalFileName}");

        try
        {
            ConsoleExtensions.WriteLine("Image Variation Create Test:", ConsoleColor.DarkCyan);

            var imageResult = await sdk.Image.CreateImageVariation(new()
            {
                Image = new(originalFileName, originalFile),
                N = 2,
                Size = ImageSize.Size256,
                ResponseFormat = ImageResponseFormat.Url,
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
                    throw new("Unknown Error");
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