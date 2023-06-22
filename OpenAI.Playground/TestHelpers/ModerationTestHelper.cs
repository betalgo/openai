using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;

namespace OpenAI.Playground.TestHelpers;

public class ModerationTestHelper
{
    public static async Task CreateModerationTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Create Moderation test is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Create Moderation test", ConsoleColor.DarkCyan);
            var moderationResponse = await sdk.Moderation.CreateModeration(new CreateModerationRequest
            {
                Input = "I want to kill them."
            });
            if (moderationResponse.Results.First().Flagged)
            {
                ConsoleExtensions.WriteLine("Create Moderation test passed.", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleExtensions.WriteLine("Create Moderation test failed", ConsoleColor.DarkRed);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}