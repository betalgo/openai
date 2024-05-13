using OpenAI.Interfaces;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static partial class AssistantTestHelper
{
    internal static partial class ThreadsTestHelper
    {
        private static string? CreatedThreadId { get; set; }

        public static async Task RunTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Thread Basics Testing is starting:", ConsoleColor.Blue);
            await CreateThread(openAI);
            await RetrieveThread(openAI);
            await ModifyThread(openAI);
            await DeleteThread(openAI);
        }

        public static async Task CreateThread(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create Thread Testing is starting:", ConsoleColor.Cyan);
            var result = await openAI.Beta.Threads.ThreadCreate(new());
            if (result.Successful)
            {
                CreatedThreadId = result.Id;
                ConsoleExtensions.WriteLine($"Thread Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task RetrieveThread(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Thread Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Threads.ThreadRetrieve(CreatedThreadId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Thread Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ModifyThread(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Modify Thread Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Threads.ModifyThread(CreatedThreadId, new()
            {
                Metadata = new()
                {
                    { "modified", "true" },
                    { "user", "abc123" }
                }
            });
            if (result.Successful)
            {
                if (result.Metadata != null && result.Metadata.ContainsKey("modified") && result.Metadata.ContainsKey("user"))
                {
                    ConsoleExtensions.WriteLine("Modify Thread Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Modify Thread Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task DeleteThread(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Delete Thread Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Threads.ThreadDelete(CreatedThreadId);
            if (result.Successful)
            {
                if (result.IsDeleted)
                {
                    ConsoleExtensions.WriteLine("Delete Thread Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Delete Thread Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }
    }
}