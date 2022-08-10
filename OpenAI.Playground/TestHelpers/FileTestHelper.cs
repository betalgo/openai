using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class FileTestHelper
    {
        public static async Task RunSimpleFileTest(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("File Testing is starting:", ConsoleColor.Cyan);

            try
            {
                const string fileName = "SentimentAnalysisSample.jsonl";

                var sampleFile = await File.ReadAllBytesAsync($"SampleData/{fileName}");

                ConsoleExtensions.WriteLine($"Uploading file {fileName}", ConsoleColor.DarkCyan);
                var uploadFilesResponse = await sdk.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.FineTune, sampleFile, fileName);
                if (uploadFilesResponse.Successful)
                {
                    ConsoleExtensions.WriteLine($"{fileName} uploaded", ConsoleColor.DarkGreen);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"{fileName} failed", ConsoleColor.DarkRed);
                }

                ConsoleExtensions.WriteLine("Listing files", ConsoleColor.Cyan);
                var uploadedFiles = await sdk.Files.FileList();
                // Need to wait for file processing before deleting it.

                ConsoleExtensions.WriteLine("Need to wait for file processing", ConsoleColor.White);
                await Task.Delay(10_000);
                foreach (var uploadedFile in uploadedFiles.Data)
                {
                    ConsoleExtensions.WriteLine($"Retrieving {uploadedFile.FileName}", ConsoleColor.DarkCyan);
                    var retrieveFileResponse = await sdk.Files.FileRetrieve(uploadedFile.Id);
                    if (retrieveFileResponse.Successful)
                    {
                        ConsoleExtensions.WriteLine($"{retrieveFileResponse.FileName} retrieved", ConsoleColor.DarkGreen);
                    }
                    else
                    {
                        ConsoleExtensions.WriteLine($"Retrieve {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                    }

                    //   var fileContent = sdk.Files.RetrieveFileContent(file.Id);
                    ConsoleExtensions.WriteLine($"Deleting file {uploadedFile.FileName}", ConsoleColor.DarkCyan);
                    var deleteResponse = await sdk.Files.FileDelete(uploadedFile.Id);
                    if (deleteResponse.Successful)
                    {
                        ConsoleExtensions.WriteLine($"{retrieveFileResponse.FileName} deleted", ConsoleColor.DarkGreen);
                    }
                    else
                    {
                        ConsoleExtensions.WriteLine($"Delete {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        public static async Task CleanAllFiles(IOpenAIService sdk)
        {
            try
            {
                Console.WriteLine("Starting to clean All Files");
                var uploadedFiles = await sdk.Files.FileList();
                foreach (var uploadedFile in uploadedFiles.Data)
                {
                    Console.WriteLine(uploadedFile.FileName);
                    var deleteResponse = await sdk.Files.FileDelete(uploadedFile.Id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}