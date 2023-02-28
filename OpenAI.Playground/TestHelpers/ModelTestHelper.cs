using OpenAI.GPT3.Interfaces;

namespace OpenAI.Playground.TestHelpers;

internal static class ModelTestHelper
{
    public static async Task FetchModelsTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Model List Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Fetching Model List", ConsoleColor.DarkCyan);
            var modelList = await sdk.Models.ListModel();
            if (modelList == null)
            {
                ConsoleExtensions.WriteLine("Fetching Model List failed", ConsoleColor.DarkRed);
                throw new NullReferenceException(nameof(modelList));
            }

            ConsoleExtensions.WriteLine("Models:", ConsoleColor.DarkGreen);
            Console.WriteLine(string.Join(Environment.NewLine, modelList.Models.Select(r => r.Id)));

            foreach (var modelItem in modelList.Models)
            {
                ConsoleExtensions.WriteLine($"Retrieving Model:{modelItem.Id}", ConsoleColor.DarkCyan);

                var retrieveModelResponse = await sdk.Models.RetrieveModel(modelItem.Id);
                if (retrieveModelResponse.Successful)
                {
                    Console.WriteLine(retrieveModelResponse);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Retrieving {modelItem.Id} Model failed", ConsoleColor.DarkRed);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}