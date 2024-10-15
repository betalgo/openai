using Betalgo.OpenAI.Interfaces;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class BatchTestHelper
{
    public static async Task RunBatchOperationsTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Batch Operations Testing is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Batch Create Test:", ConsoleColor.DarkCyan);

            const string fileName = "BatchDataSampleFile.jsonl";
            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");
            ConsoleExtensions.WriteLine($"Uploading file {fileName}", ConsoleColor.DarkCyan);

            var fileUploadResult = await sdk.Files.UploadFile("batch", sampleFile, fileName);

            if (!fileUploadResult.Successful)
            {
                throw new("File upload failed");
            }

            var batchCreateResult = await sdk.Batch.BatchCreate(new()
            {
                InputFileId = fileUploadResult.Id,
                Endpoint = "/v1/chat/completions",
                CompletionWindow = "24h"
            });

            if (!batchCreateResult.Successful)
            {
                throw new("Batch creation failed");
            }

            ConsoleExtensions.WriteLine($"Batch ID: {batchCreateResult.Id}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"Batch Status: {batchCreateResult.Status}", ConsoleColor.Green);

            ConsoleExtensions.WriteLine("Batch Retrieve Test:", ConsoleColor.DarkCyan);

            var batchRetrieveResult = await sdk.Batch.BatchRetrieve(batchCreateResult.Id);

            if (!batchRetrieveResult.Successful)
            {
                throw new("Batch retrieval failed");
            }

            ConsoleExtensions.WriteLine($"Batch ID: {batchRetrieveResult.Id}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"Batch Status: {batchRetrieveResult.Status}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"Request Counts:", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"  Total: {batchRetrieveResult.RequestCounts.Total}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"  Completed: {batchRetrieveResult.RequestCounts.Completed}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"  Failed: {batchRetrieveResult.RequestCounts.Failed}", ConsoleColor.Green);

            ConsoleExtensions.WriteLine("Batch Cancel Test:", ConsoleColor.DarkCyan);

            var batchCancelResult = await sdk.Batch.BatchCancel(batchCreateResult.Id);

            if (!batchCancelResult.Successful)
            {
                throw new("Batch cancellation failed");
            }

            ConsoleExtensions.WriteLine($"Batch ID: {batchCancelResult.Id}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"Batch Status: {batchCancelResult.Status}", ConsoleColor.Green);
            ConsoleExtensions.WriteLine($"Cancelling At: {batchCancelResult.CancellingAt}", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            ConsoleExtensions.WriteLine($"Error: {e.Message}", ConsoleColor.Red);
            throw;
        }
    }
}