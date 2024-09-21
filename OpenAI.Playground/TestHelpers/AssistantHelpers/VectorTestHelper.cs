using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.ResponseModels.VectorStoreResponseModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers.AssistantHelpers;

internal static partial class AssistantTestHelper
{
    internal static class VectorTestHelper
    {
        private static string? CreatedVectorId { get; set; }
        private static string? CreatedVectorFileId { get; set; }
        private static string? CreatedVectorFileBatchId { get; set; }
        private static string? CreatedFileId1 { get; set; }
        private static string? CreatedFileId2 { get; set; }


        public static async Task RunTests(IOpenAIService openAI)
        {
            await RunBasicTests(openAI);
            await RunFileTests(openAI);
            await RunBatchTests(openAI);
        }

        public static async Task RunBasicTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Vector Basics Testing is starting:", ConsoleColor.Blue);
            await CreateVector(openAI);
            await ListVectors(openAI);
            await RetrieveVector(openAI);
            await ModifyVector(openAI);
            await DeleteVector(openAI);
        }

        public static async Task RunFileTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Vector File Testing is starting:", ConsoleColor.Blue);
            await CreateVector(openAI);
            await CreateVectorFile(openAI);
            await ListVectorFiles(openAI);
            await RetrieveVectorFile(openAI);
            await DeleteVectorFile(openAI);
            await Cleanup(openAI);
        }

        public static async Task RunBatchTests(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Vector Batch File Testing is starting:", ConsoleColor.Blue);
            await CreateVectorFileBatch(openAI);
            await ListVectorStoreFileBatches(openAI);
            await RetrieveVectorStoreFileBatch(openAI);
            await CancelVectorStoreFileBatch(openAI);
            await Cleanup(openAI);
        }

        public static async Task CreateVector(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create Vector Testing is starting:", ConsoleColor.Cyan);
            var result = await openAI.Beta.VectorStores.CreateVectorStore(new()
            {
                Name = "Support FAQ"
            });

            if (result.Successful)
            {
                CreatedVectorId = result.Id;
                ConsoleExtensions.WriteLine($"Vector Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CreateVectorWithChunkingStrategy(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create Vector Testing is starting:", ConsoleColor.Cyan);
            var result = await openAI.Beta.VectorStores.CreateVectorStore(new()
            {
                Name = "Support FAQ",
                ChunkingStrategy = new()
                {
                    Type = StaticValues.VectorStoreStatics.ChunkingStrategyType.Static,
                    StaticParameters = new()
                    {
                        ChunkOverlapTokens = 400,
                        MaxChunkSizeTokens = 800
                    }
                }
            });

            if (result.Successful)
            {
                CreatedVectorId = result.Id;
                ConsoleExtensions.WriteLine($"Vector Created Successfully with ID: {result.Id}", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListVectors(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Vectors Testing is starting:", ConsoleColor.Cyan);
            var allVectors = new List<VectorStoreObjectResponse>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.VectorStores.ListVectorStores(new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allVectors.AddRange(result.Data);
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

            foreach (var vector in allVectors)
            {
                Console.WriteLine($"ID: {vector.Id}, Name: {vector.Name}");
            }

            if (allVectors.FirstOrDefault(r => r.Id == CreatedVectorId) != null)
            {
                ConsoleExtensions.WriteLine("List Vectors Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("List Vectors Test is failed.", ConsoleColor.Red);
            }
        }

        public static async Task RetrieveVector(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Vector Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.VectorStores.RetrieveVectorStore(CreatedVectorId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Vector Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ModifyVector(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Modify Vector Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            const string newName = "Support FAQ 2";
            var result = await openAI.Beta.VectorStores.ModifyVectorStore(CreatedVectorId, new()
            {
                Name = newName
            });

            if (result.Successful)
            {
                if (result.Name == newName)
                {
                    ConsoleExtensions.WriteLine("Modify Vector Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Modify Vector Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task DeleteVector(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Delete Vector Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.VectorStores.DeleteVectorStore(CreatedVectorId);
            if (result.Successful)
            {
                if (result.IsDeleted)
                {
                    ConsoleExtensions.WriteLine("Delete Vector Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Delete Vector Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CreateVectorFile(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Create Vector File Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            const string fileName = "HowAssistantsWork.txt";
            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");

            ConsoleExtensions.WriteLine($"Uploading file {fileName}", ConsoleColor.DarkCyan);
            var uploadFilesResponse = await openAI.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Assistants, sampleFile, fileName);
            if (uploadFilesResponse.Successful)
            {
                ConsoleExtensions.WriteLine($"{fileName} uploaded", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleExtensions.WriteError(uploadFilesResponse.Error);
            }

            CreatedFileId1 = uploadFilesResponse.Id;
            var result = await openAI.Beta.VectorStoreFiles.CreateVectorStoreFile(CreatedVectorId, new()
            {
                FileId = CreatedFileId1
            });

            if (result.Successful)
            {
                CreatedVectorFileId = result.Id;
                ConsoleExtensions.WriteLine("Create Vector File Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListVectorFiles(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Vector Files Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            var allFiles = new List<VectorStoreFileObject>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.VectorStoreFiles.ListVectorStoreFiles(CreatedVectorId, new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allFiles.AddRange(result.Data);
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

            foreach (var file in allFiles)
            {
                Console.WriteLine($"ID: {file.Id}, Vector Store ID: {file.VectorStoreId}");
            }

            if (allFiles.FirstOrDefault(r => r.Id == CreatedVectorFileId) != null)
            {
                ConsoleExtensions.WriteLine("List Vector Files Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("List Vector Files Test is failed.", ConsoleColor.Red);
            }
        }

        public static async Task RetrieveVectorFile(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Vector File Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorFileId))
            {
                ConsoleExtensions.WriteLine("Vector File Id is not found. Please add a file to the vector first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.VectorStoreFiles.GetVectorStoreFile(CreatedVectorId, CreatedVectorFileId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Vector File Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task DeleteVectorFile(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Delete Vector File Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorFileId))
            {
                ConsoleExtensions.WriteLine("Vector File Id is not found. Please add a file to the vector first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.VectorStoreFiles.DeleteVectorStoreFile(CreatedVectorId, CreatedVectorFileId);
            if (result.Successful)
            {
                if (result.IsDeleted)
                {
                    ConsoleExtensions.WriteLine("Delete Vector File Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Delete Vector File Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CreateVectorFileBatch(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Vector Store File Batch Testing is starting:", ConsoleColor.Cyan);

            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            const string fileName1 = "Assistants-overview.html";
            const string fileName2 = "How-Assistants-work.html";

            var file1 = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName1}");
            var file2 = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName2}");

            ConsoleExtensions.WriteLine("Uploading files", ConsoleColor.DarkCyan);
            var uploadFilesResponse1 = await openAI.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Assistants, file1, fileName1);
            var uploadFilesResponse2 = await openAI.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Assistants, file2, fileName2);

            if (uploadFilesResponse1.Successful && uploadFilesResponse2.Successful)
            {
                ConsoleExtensions.WriteLine("Files uploaded", ConsoleColor.DarkGreen);
            }
            else
            {
                ConsoleExtensions.WriteError(uploadFilesResponse1.Error);
                ConsoleExtensions.WriteError(uploadFilesResponse2.Error);
                return;
            }

            CreatedFileId1 = uploadFilesResponse1.Id;
            CreatedFileId2 = uploadFilesResponse2.Id;
            var result = await openAI.Beta.VectorStoreFiles.CreateVectorStoreFileBatch(CreatedVectorId, new()
            {
                FileIds = [uploadFilesResponse1.Id, uploadFilesResponse2.Id]
            });

            if (result.Successful)
            {
                CreatedVectorFileBatchId = result.Id;
                ConsoleExtensions.WriteLine("Vector Store File Batch Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task ListVectorStoreFileBatches(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("List Vector Store File Batches Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedVectorFileBatchId))
            {
                ConsoleExtensions.WriteLine("Vector File Batch Id is not found. Please create a vector file batch first.", ConsoleColor.Red);
                return;
            }

            var allBatches = new List<VectorStoreFileBatchObject>();
            var hasMore = true;
            var lastId = string.Empty;
            while (hasMore)
            {
                var result = await openAI.Beta.VectorStoreFiles.ListFilesInVectorStoreBatch(CreatedVectorId, CreatedVectorFileBatchId, new() { After = lastId, Limit = 5 });
                if (result.Successful)
                {
                    if (result.Data != null)
                    {
                        allBatches.AddRange(result.Data);
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

            foreach (var batch in allBatches)
            {
                Console.WriteLine($"ID: {batch.Id}, Vector Store ID: {batch.VectorStoreId}");
            }

            if (allBatches.FirstOrDefault(r => r.Id == CreatedVectorFileBatchId) != null)
            {
                ConsoleExtensions.WriteLine("List Vector Store File Batches Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLine("List Vector Store File Batches Test is failed.", ConsoleColor.Red);
            }
        }

        public static async Task RetrieveVectorStoreFileBatch(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Retrieve Vector Store File Batch Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedVectorFileBatchId))
            {
                ConsoleExtensions.WriteLine("Vector File Batch Id is not found. Please create a vector file batch first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.VectorStoreFiles.GetVectorStoreFileBatch(CreatedVectorId, CreatedVectorFileBatchId);
            if (result.Successful)
            {
                ConsoleExtensions.WriteLine("Retrieve Vector Store File Batch Test is successful.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        public static async Task CancelVectorStoreFileBatch(IOpenAIService openAI)
        {
            ConsoleExtensions.WriteLine("Cancel Vector Store File Batch Testing is starting:", ConsoleColor.Cyan);
            if (string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                ConsoleExtensions.WriteLine("Vector Id is not found. Please create a vector first.", ConsoleColor.Red);
                return;
            }

            if (string.IsNullOrWhiteSpace(CreatedVectorFileBatchId))
            {
                ConsoleExtensions.WriteLine("Vector File Batch Id is not found. Please create a vector file batch first.", ConsoleColor.Red);
                return;
            }

            var result = await openAI.Beta.VectorStoreFiles.CancelVectorStoreFileBatch(CreatedVectorId, CreatedVectorFileBatchId);
            if (result.Successful)
            {
                if (result.Status == "cancelling")
                {
                    ConsoleExtensions.WriteLine("Cancel Vector Store File Batch Test is successful.", ConsoleColor.Green);
                }
                else
                {
                    ConsoleExtensions.WriteLine("Cancel Vector Store File Batch Test is failed.", ConsoleColor.Red);
                }
            }
            else
            {
                ConsoleExtensions.WriteError(result.Error);
            }
        }

        private static async Task Cleanup(IOpenAIService sdk)
        {
            if (!string.IsNullOrWhiteSpace(CreatedVectorFileId) && !string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                await sdk.Beta.VectorStoreFiles.DeleteVectorStoreFile(CreatedVectorId, CreatedVectorFileId);
            }

            if (!string.IsNullOrWhiteSpace(CreatedFileId1))
            {
                await sdk.Files.DeleteFile(CreatedFileId1);
            }

            if (!string.IsNullOrWhiteSpace(CreatedFileId2))
            {
                await sdk.Files.DeleteFile(CreatedFileId2);
            }

            if (!string.IsNullOrWhiteSpace(CreatedVectorId))
            {
                await sdk.Beta.VectorStores.DeleteVectorStore(CreatedVectorId);
            }
        }
    }
}