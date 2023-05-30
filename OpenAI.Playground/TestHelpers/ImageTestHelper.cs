using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers;

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
                Prompt = "Laser cat eyes",
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

    public static async Task RunSimpleCreateImageEditTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Image Edit Create Testing is starting:", ConsoleColor.Cyan);
        const string maskFileName = "image_edit_mask.png";
        const string originalFileName = "image_edit_original.png";

        // Images should be in png format with ARGB. I got help from this website to generate sample mask
        // https://www.online-image-editor.com/
        var maskFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{maskFileName}");
        var originalFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{originalFileName}");

        try
        {
            ConsoleExtensions.WriteLine("Image  Edit Create Test:", ConsoleColor.DarkCyan);
            var imageResult = await sdk.Image.CreateImageEdit(new ImageEditCreateRequest
            {
                Image = originalFile,
                ImageName = originalFileName,
                Mask = maskFile,
                MaskName = maskFileName,
                Prompt = "A sunlit indoor lounge area with a pool containing a cat",
                N = 4,
                Size = StaticValues.ImageStatics.Size.Size1024,
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

    public static async Task RunSimpleCreateImageVariationTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Image Variation Create Testing is starting:", ConsoleColor.Cyan);
        const string originalFileName = "image_edit_original.png";

        var originalFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{originalFileName}");

        try
        {
            ConsoleExtensions.WriteLine("Image Variation Create Test:", ConsoleColor.DarkCyan);
            var imageResult = await sdk.Image.CreateImageVariation(new ImageVariationCreateRequest
            {
                Image = originalFile,
                ImageName = originalFileName,
                N = 2,
                Size = StaticValues.ImageStatics.Size.Size1024,
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