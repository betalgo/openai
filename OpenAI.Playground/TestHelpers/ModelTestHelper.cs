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
            var engineList = await sdk.Models.ListModel();
            if (engineList == null)
            {
                ConsoleExtensions.WriteLine("Fetching Model List failed", ConsoleColor.DarkRed);
                throw new NullReferenceException(nameof(engineList));
            }

            ConsoleExtensions.WriteLine("Models:", ConsoleColor.DarkGreen);
            Console.WriteLine(string.Join(Environment.NewLine, engineList.Models.Select(r => r.Id)));

            foreach (var engineItem in engineList.Models)
            {
                ConsoleExtensions.WriteLine($"Retrieving Model:{engineItem.Id}", ConsoleColor.DarkCyan);

                var retrieveEngineResponse = await sdk.Models.RetrieveModel(engineItem.Id);
                if (retrieveEngineResponse.Successful)
                {
                    Console.WriteLine(retrieveEngineResponse);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Retrieving {engineItem.Id} Model failed", ConsoleColor.DarkRed);
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