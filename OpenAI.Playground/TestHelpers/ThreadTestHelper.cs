using Betalgo.OpenAI.Interfaces;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class ThreadTestHelper
{
    public static async Task<string> RunThreadCreateTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Thread create Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Thread Create Test:", ConsoleColor.DarkCyan);
            var threadResult = await sdk.Beta.Threads.ThreadCreate();

            if (threadResult.Successful)
            {
                ConsoleExtensions.WriteLine(threadResult.ToJson());
                return threadResult.Id;
            }
            else
            {
                if (threadResult.Error == null)
                {
                    throw new("Unknown Error");
                }

                ConsoleExtensions.WriteLine($"{threadResult.Error.Code}: {threadResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        throw new InvalidOperationException();
    }

    public static async Task RunThreadRetrieveTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Thread retrieve Testing is starting:", ConsoleColor.Cyan);
        try
        {
            ConsoleExtensions.WriteLine("Thread Retrieve Test:", ConsoleColor.DarkCyan);
            var threadId = await RunThreadCreateTest(sdk);
            var threadResult = await sdk.Beta.Threads.ThreadRetrieve(threadId);

            if (threadResult.Successful)
            {
                ConsoleExtensions.WriteLine(threadResult.ToJson());
            }
            else
            {
                if (threadResult.Error == null)
                {
                    throw new("Unknown Error");
                }

                ConsoleExtensions.WriteLine($"{threadResult.Error.Code}: {threadResult.Error.Message}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}