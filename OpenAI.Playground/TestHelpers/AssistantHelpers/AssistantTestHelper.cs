using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static partial class AssistantTestHelper
{
    internal static class BasicsTestHelper
    {
        private const string Instruction = "You are a personal math tutor. When asked a question, write and run Python code to answer the question.";
        private const string Name = "Math Tutor";
        private static string? CreatedAssistantId { get; set; }

        public static async Task RunTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Assistant Basics Testing is starting:", ConsoleColor.Blue);
            await CreateAssistant(openAI);
            await ListAssistants(openAI);
            await RetrieveAssistant(openAI);
            await ModifyAssistantTask(openAI);
            await DeleteAssistant(openAI);
        }

        public static async Task CreateAssistant(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create Assistant Testing is starting:", ConsoleColor.Cyan);

            var result = await openAI.Beta.Assistants.AssistantCreate(new()
            {
                Instructions = Instruction,
                Name = Name,
                Tools = [ToolDefinition.DefineCodeInterpreter()],
                Model = Models.Gpt_4_turbo
            });

            if (result.Successful)
            {
                CreatedAssistantId = result.Id;
                ConsoleExtensions.WriteLine($"Assistant Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListAssistants(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Assistants Testing is starting:", ConsoleColor.Cyan);
            var allAssistants = new List<AssistantResponse>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.Assistants.AssistantList(new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allAssistants.AddRange(result.Data);
                    }

                    hasMore = result.HasMore;
                    lastId = result.LastId;
                }
                else
                {
                    ConsoleExtensions.WriteError(result.Error);
                    return;
                }
            }

            foreach (var assistant in allAssistants)
            {
                Console.WriteLine($"ID: {assistant.Id}, Name: {assistant.Name}");
            }

            if (allAssistants.FirstOrDefault(r => r.Id == CreatedAssistantId) != null)
            {
                ConsoleExtensions.WriteLine("List Assistants Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("List Assistants Test is failed.", ConsoleColor.Red);
            }
        }

        public static async Task RetrieveAssistant(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Assistant Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedAssistantId))
            {
                ConsoleExtensions.WriteLine("Assistant Id is not found. Please create an assistant first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Assistants.AssistantRetrieve(CreatedAssistantId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Assistant Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ModifyAssistantTask(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Modify Assistant Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedAssistantId))
            {
                ConsoleExtensions.WriteLine("Assistant Id is not found. Please create an assistant first.", ConsoleColor.Red);
                return;
            }

            const string newName = Name + "2";
            const string newInstructions = "You are an HR bot, and you have access to files to answer employee questions about company policies. Always response with info from either of the files.";
            var result = await openAI.Beta.Assistants.AssistantModify(CreatedAssistantId, new()
            {
                Instructions = newInstructions,
                Name = newName,
                Tools = [ToolDefinition.DefineFileSearch()],
                Model = Models.Gpt_4_turbo
            });

            if (result.Successful)
            {
                if (result is { Name: newName, Instructions: newInstructions } && result.Tools.First().Type == ToolDefinition.DefineFileSearch().Type)
                {
                    ConsoleExtensions.WriteLine("Modify Assistant Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Modify Assistant Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task DeleteAssistant(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Delete Assistant Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedAssistantId))
            {
                ConsoleExtensions.WriteLine("Assistant Id is not found. Please create an assistant first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Assistants.AssistantDelete(CreatedAssistantId);
            if (result.Successful)
            {
                if (result.IsDeleted)
                {
                    ConsoleExtensions.WriteLine("Delete Assistant Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Delete Assistant Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }
    }
}