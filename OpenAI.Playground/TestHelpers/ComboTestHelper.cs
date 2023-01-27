using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System.Diagnostics;

namespace OpenAI.Playground.TestHelpers
{
    internal static class ComboTestHelper
    {
        /// <summary>
        /// Runs a combo test with a prompt for the completion and then use the entire completion for image generation
        /// </summary>
        /// <param name="sdk"></param>
        /// <param name="completionPrompt"></param>
        /// <returns></returns>
        public static async Task RunSimpleComboTest(IOpenAIService sdk, string completionPrompt)
        {
            ConsoleExtensions.WriteLine("Combo Completion/Image Test:", ConsoleColor.Cyan);
            try
            {
                var imagePrompt = await CompletionTestHelper.RunSimpleCompletionStreamTest(sdk, completionPrompt);
                var imageUrls = await ImageTestHelper.RunSimpleCreateImageTest(sdk, imagePrompt, 4);
                var html = await BuildHtml(completionPrompt, imagePrompt, imageUrls);
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ComboCompletionImageTest.html");
                await File.WriteAllTextAsync(path, html);
                Console.WriteLine("HTML file saved to " + path);

                var uri = new Uri(path);
                var url = uri.AbsoluteUri;

                Process.Start(new ProcessStartInfo(url){UseShellExecute = true});

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task<string> BuildHtml(string completionPrompt, string imagePrompt, List<string> imageUrls)
        {
            var body = $"<h1>Completion</h1>";
            body += "<p><strong>The initial completion prompt was:</strong> {completionPrompt}</p>";
            body += $"<p>This gave us the following full completion from OpenAI: {imagePrompt}</p>";
            body += $"<h1>Image Generation</h1>";
            body +=
                $"<p>We then fed the full completion into image generation as a prompt and got back the following images:</p>";
            foreach (var imageUrl in imageUrls)
            {
                body += $"<img src={imageUrl} alt={imagePrompt}><br/>";
            }
            var html = $"<!DOCTYPE html>\n<html>\n<head>\n<title>{completionPrompt}</title>\n</head>\n<body>{body}</body>\n</html>";
            return html;
        }
    }
}
