using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class MessageTestHelper
{
    public static async Task RunMessageCreateTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Message create Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Message Create Test:", ConsoleColor.DarkCyan);

            var threadResult = await sdk.Beta.Threads.ThreadCreate();
            if (threadResult.Successful)
            {
                ConsoleExtensions.WriteLine(threadResult.ToJson());
            }
            else
            {
                if (threadResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                ConsoleExtensions.WriteLine($"{threadResult.Error.Code}: {threadResult.Error.Message}");
            }

            var threadId = threadResult.Id;
            ConsoleExtensions.WriteLine($"threadId :{threadId}");

            var messageResult = await sdk.Beta.Messages.MessageCreate(threadId, new MessageCreateRequest
            {
                Role = StaticValues.AssistantsStatics.MessageStatics.Roles.User,
                Content = "How does AI work? Explain it in simple terms."
            });

            if (messageResult.Successful)
            {
                ConsoleExtensions.WriteLine(messageResult.ToJson());
            }
            else
            {
                if (messageResult.Error == null)
                {
                    throw new Exception("Unknown Error");
                }

                ConsoleExtensions.WriteLine($"{messageResult.Error.Code}: {messageResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}