using OpenAI.SDK.Interfaces;
using OpenAI.SDK.Models;
using OpenAI.SDK.Models.RequestModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class AnswerTestHelper
    {
        public static async Task RunSimpleAnswerTest(IOpenAISdk sdk)
        {
            ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Fetching Engine List", ConsoleColor.DarkCyan);
                var completionResult = await sdk.Answers.Answer(new AnswerCreateRequest()
                {
                    Documents = new List<string>()
                    {
                        "Puppy A is happy.", "Puppy B is sad."
                    },
                    Question = "which puppy is happy?",
                    SearchModel = Engines.Engine.Ada.EnumToString(),
                    Model = Engines.Engine.Curie.EnumToString(),
                    ExamplesContext = "In 2017, U.S. life expectancy was 78.6 years.",
                    Examples = new List<List<string>>()
                    {
                        new()
                        {
                            "What is human life expectancy in the United States?", "78 years."
                        }
                    },
                    MaxTokens = 5
                    //Stop = new List<string>()
                    //{
                    //    "\n", "<|endoftext|>"
                    //}
                });

                if (completionResult.Answers.FirstOrDefault() != "puppy A.")
                {
                    throw new Exception("Something Wrong");
                }

                Console.WriteLine(completionResult.Answers.FirstOrDefault());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}