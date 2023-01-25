using NUnit.Framework;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OpenAI.Tests
{
    public class OpenAITests
    {
        public OpenAIService openAiService { get; set; }
        [OneTimeSetUp]
        public void Setup()
        {
            openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY")
            });
        }

        [Test]
        public async Task OpenAI_CreateCompletion()
        {
            var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
            {
                Prompt = "Once upon a time",
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

            Assert.IsTrue(completionResult.Successful);
        }

        [Test]
        public async Task OpenAI_CreateImage()
        {
            var imageResult = await openAiService.Image.CreateImage(new ImageCreateRequest
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

            Assert.IsTrue(imageResult.Successful);
            Assert.That(imageResult.Results.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task OpenIA_CreateCompletionAsStream()
        {
            var completionResult = openAiService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
            {
                Prompt = "Once upon a time",
                MaxTokens = 50
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
                Assert.IsTrue(completion.Successful);
            }
        }
    }
}