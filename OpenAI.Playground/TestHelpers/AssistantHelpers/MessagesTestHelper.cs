using Betalgo.Ranul.OpenAI.Contracts.Enums;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static partial class AssistantTestHelper
{
    internal static partial class MessagesTestHelper
    {
        private static string? CreatedMessageId { get; set; }
        private static string? CreatedThreadId { get; set; }
        private static string? CreatedFileId { get; set; }

        public static async Task RunTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Message Basics Testing is starting:", ConsoleColor.Blue);
            await CreateMessage(openAI);
            await CreateMessageWithImage(openAI);
            await ListMessages(openAI);
            await RetrieveMessage(openAI);
            await ModifyMessage(openAI);
            await DeleteMessage(openAI);
            await Cleanup(openAI);
        }

        public static async Task CreateMessage(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create Message Testing is starting:", ConsoleColor.Cyan);
            var thread = await openAI.Beta.Threads.ThreadCreate();
            if (!thread.Successful)
            {
                if (thread.Error == null)
                {
                    throw new("Unknown Error");
                }

                ConsoleExtensions.WriteLine($"{thread.Error.Code}: {thread.Error.Message}", ConsoleColor.Red);
                return;
            }

            CreatedThreadId = thread.Id;
            var result = await openAI.Beta.Messages.CreateMessage(CreatedThreadId, new(AssistantMessageRole.User, new("How does AI work? Explain it in simple terms.")));
            if (result.Successful)
            {
                CreatedMessageId = result.Id;
                ConsoleExtensions.WriteLine($"Message Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CreateMessageWithImage(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create MessageWithImage Testing is starting:", ConsoleColor.Cyan);

            var prompt = "Tell me about this image";
            var filename = "image_edit_original.png";
            var filePath = $"SampleData/{filename}";

            var sampleBytes = await FileExtensions.ReadAllBytesAsync(filePath);

            // Upload File
            ConsoleExtensions.WriteLine("Upload File Test", ConsoleColor.DarkCyan);

            ConsoleExtensions.WriteLine($"Uploading file: {filename}", ConsoleColor.DarkCyan);
            var uploadFilesResponse = await openAI.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Vision, sampleBytes, filename);
            if (uploadFilesResponse.Successful)
            {
                ConsoleExtensions.WriteLine($"{filename} uploaded", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleExtensions.WriteLine($"{filename} failed", ConsoleColor.DarkRed);
                return;
            }

            var uploadFileId = uploadFilesResponse.Id;
            ConsoleExtensions.WriteLine($"uploadFileId:{uploadFileId}, purpose:{uploadFilesResponse.Purpose}");


            // Message.ImageFileContent
            ConsoleExtensions.WriteLine("Message with ImageFileContent Test:", ConsoleColor.DarkCyan);

            MessageContentOneOfType content = new([
                MessageContent.TextContent(prompt),
                MessageContent.ImageFileContent(uploadFileId,ImageDetailType.High)
            ]);

            MessageCreateRequest request = new()
            {
                Role = AssistantMessageRole.User,
                Content = content
            };

            var result = await openAI.Beta.Messages.CreateMessage(CreatedThreadId!, request);
            if (result.Successful)
            {
                CreatedMessageId = result.Id;
                ConsoleExtensions.WriteLine($"Message Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListMessages(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Messages Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var allMessages = new List<MessageResponse>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.Messages.ListMessages(CreatedThreadId, new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allMessages.AddRange(result.Data);
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

            foreach (var message in allMessages)
            {
                Console.WriteLine($"ID: {message.Id}, Content: {string.Join("", message.Content?.Select(r => r.Text?.Value) ?? [])}");
            }

            if (allMessages.FirstOrDefault(r => r.Id == CreatedMessageId) != null)
            {
                ConsoleExtensions.WriteLine("List Messages Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("List Messages Test is failed.", ConsoleColor.Red);
            }
        }

        public static async Task RetrieveMessage(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Message Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedMessageId))
            {
                ConsoleExtensions.WriteLine("Message Id is not found. Please create a message first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Messages.RetrieveMessage(CreatedThreadId, CreatedMessageId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Message Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ModifyMessage(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Modify Message Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedMessageId))
            {
                ConsoleExtensions.WriteLine("Message Id is not found. Please create a message first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Messages.ModifyMessage(CreatedThreadId, CreatedMessageId, new()
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
                    ConsoleExtensions.WriteLine("Modify Message Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Modify Message Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task DeleteMessage(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Delete Message Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedMessageId))
            {
                ConsoleExtensions.WriteLine("Message Id is not found. Please create a message first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                ConsoleExtensions.WriteLine("Thread Id is not found. Please create a thread first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.Messages.DeleteMessage(CreatedThreadId, CreatedMessageId);
            if (result.Successful)
            {
                if (result.IsDeleted)
                {
                    ConsoleExtensions.WriteLine("Delete Message Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Delete Message Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        private static async Task Cleanup(IOpenAIService sdk)
        {
            if (!string.IsNullOrWhiteSpace(CreatedThreadId))
            {
                await sdk.Beta.Threads.ThreadDelete(CreatedThreadId);
            }

            if (!string.IsNullOrWhiteSpace(CreatedFileId))
            {
                await sdk.Files.DeleteFile(CreatedFileId);
            }
        }
    }
}