using System.Text;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels;

namespace OpenAI.Playground.TestHelpers;

internal static class FileTestHelper
{
    public static async Task RunSimpleFileTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("File Testing is starting:", ConsoleColor.Cyan);

        try
        {
            const string fileName = "SentimentAnalysisSample.jsonl";

            var sampleFile = await FileExtensions.ReadAllBytesAsync($"SampleData/{fileName}");
            var sampleFileAsString = Encoding.UTF8.GetString(sampleFile);

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
            var uploadedFiles = await sdk.Files.ListFile();
            // Need to wait for file processing before deleting it.

            ConsoleExtensions.WriteLine("Need to wait for file processing", ConsoleColor.White);
            await Task.Delay(10_000);
            foreach (var uploadedFile in uploadedFiles.Data)
            {
                ConsoleExtensions.WriteLine($"Retrieving {uploadedFile.FileName}", ConsoleColor.DarkCyan);
                var retrieveFileResponse = await sdk.Files.RetrieveFile(uploadedFile.Id);
                if (retrieveFileResponse.Successful)
                {
                    ConsoleExtensions.WriteLine($"{retrieveFileResponse.FileName} retrieved", ConsoleColor.DarkGreen);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Retrieve {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                }

                var retrieveFileContentResponse = await sdk.Files.RetrieveFileContent(uploadedFile.Id);
                if (retrieveFileContentResponse.Successful && retrieveFileContentResponse.Content?.Equals(sampleFileAsString) == true)
                {
                    ConsoleExtensions.WriteLine($"retrieved content as string:{Environment.NewLine}{retrieveFileContentResponse.Content} ", ConsoleColor.DarkGreen);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Retrieve {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                }

                var retrieveFileContentResponseAsByteArray = await sdk.Files.RetrieveFileContent<byte[]>(uploadedFile.Id);
                if (retrieveFileContentResponseAsByteArray.Content != null && sampleFileAsString == Encoding.UTF8.GetString(retrieveFileContentResponseAsByteArray.Content))
                {
                    ConsoleExtensions.WriteLine($"retrieved content as byteArray:{Environment.NewLine}{Encoding.UTF8.GetString(retrieveFileContentResponseAsByteArray.Content)} ", ConsoleColor.DarkGreen);
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Retrieve {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                }

                var retrieveFileContentResponseAsStream = await sdk.Files.RetrieveFileContent<Stream>(uploadedFile.Id);

                if (retrieveFileContentResponseAsStream.Content != null)
                {
                    var reader = new StreamReader(retrieveFileContentResponseAsStream.Content!);
                    var content = await reader.ReadToEndAsync();
                    if (content.Equals(sampleFileAsString))
                    {
                        ConsoleExtensions.WriteLine($"retrieved content as Stream:{Environment.NewLine}{content} ", ConsoleColor.DarkGreen);
                    }
                    else
                    {
                        ConsoleExtensions.WriteLine($"Retrieve {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                    }
                }
                else
                {
                    ConsoleExtensions.WriteLine($"Retrieve {retrieveFileResponse.FileName} failed", ConsoleColor.Red);
                }

                //   var fileContent = sdk.Files.RetrieveFileContent(file.Id);
                ConsoleExtensions.WriteLine($"Deleting file {uploadedFile.FileName}", ConsoleColor.DarkCyan);
                var deleteResponse = await sdk.Files.DeleteFile(uploadedFile.Id);
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
            var uploadedFiles = await sdk.Files.ListFile();
            foreach (var uploadedFile in uploadedFiles.Data)
            {
                Console.WriteLine(uploadedFile.FileName);
                var deleteResponse = await sdk.Files.DeleteFile(uploadedFile.Id);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}