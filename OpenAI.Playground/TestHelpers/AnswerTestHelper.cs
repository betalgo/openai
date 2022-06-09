using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class AnswerTestHelper
    {
        public static async Task RunSimpleAnswerTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Completion Testing is starting:", ConsoleColor.Cyan);

            try
            {
                ConsoleExtensions.WriteLine("Fetching Engine List", ConsoleColor.DarkCyan);
                var completionResult = await sdk.Answers.Answer(new AnswerCreateRequest(
                    "which puppy is happy?",
                    new List<List<string>>
                    {
                        new() {"What is human life expectancy in the United States?", "78 years."}
                    },
                    "In 2017, U.S. life expectancy was 78.6 years.",
                    Models.Curie)
                {
                    Documents = new List<string>()
                    {
                        "Puppy A is happy.", "Puppy B is sad."
                    },
                    SearchModel = Models.Ada,
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