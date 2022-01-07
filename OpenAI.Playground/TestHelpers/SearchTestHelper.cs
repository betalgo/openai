using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.Models;
using OpenAI.GPT3.Models.RequestModels;

namespace OpenAI.Playground.TestHelpers
{
    internal static class SearchTestHelper
    {
        public static async Task SearchDocuments(IOpenAIService sdk)
        {
            ConsoleExtensions.WriteLine("Search Documents Test is starting:", ConsoleColor.Cyan);

            try
            {
                var documents = new List<string>()
                {
                    "White House",
                    "hospital",
                    "school"
                };
                var searchResponse = await sdk.Searches.SearchCreate(new SearchCreateRequest
                {
                    Documents = documents,
                    Query = "the president"
                }, Engines.Engine.Davinci);

                if (searchResponse.Data.FirstOrDefault()!.Document != 0)
                {
                    throw new Exception("something wrong");
                }

                Console.WriteLine(documents[searchResponse.Data.FirstOrDefault()!.Document]);
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLine(e.Message, ConsoleColor.Red);
                throw;
            }
        }

        public static async Task UploadSampleFileAndGetSearchResponse(IOpenAIService sdk)
        {
            const string fileName = "SearchSample.jsonl";

            try
            {
                Console.WriteLine($"Starting to read {fileName}");
                var searchSampleFile = await File.ReadAllBytesAsync($"SampleData/{fileName}");
                Console.WriteLine($"Uploading to read {fileName}");
                var uploadResult = await sdk.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Search.EnumToString(), searchSampleFile, fileName);
                if (uploadResult?.Successful == true)
                {
                    Console.WriteLine($"Uploading is done.");
                    Console.WriteLine($"File name:{uploadResult.FileName}");
                    Console.WriteLine($"File id:{uploadResult.Id}");
                    Console.WriteLine($"File purpose:{uploadResult.Purpose}");
                }

                Console.WriteLine($"Fetching files.");
                var uploadedFiles = await sdk.Files.FileList();
                var uploadedFile = uploadedFiles!.Data.Single(r => r.Id == uploadResult.Id);
                Console.WriteLine($"File found.");
                var file = await sdk.Files.FileRetrieve(uploadedFile.Id);
                Console.WriteLine($"File retrieved.{file.CreatedAt}");
                //var deleteResponse = await sdk.Files.DeleteFile(uploadedFile.Id);
                //if (deleteResponse?.Successful == true && deleteResponse.Deleted)
                //{
                //    Console.WriteLine($"File deleted.");
                //}
                //else
                //{
                //    Console.WriteLine($"Something went wrong while deleting file.");
                //}
                var searchResponse = await sdk.Searches.SearchCreate(new SearchCreateRequest()
                {
                    File = uploadedFile.Id,
                    MaxRerank = 5,
                    Query = "happy",
                    SearchModel = Engines.Engine.Ada.EnumToString()
                }, Engines.Engine.Ada);
                if (searchResponse?.Successful == true)
                {
                    //Console.WriteLine(string.Join(",", searchResponse.Data));
                }
                else
                {
                    Console.WriteLine($"Something went wrong while creating a search.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task UploadSampleFile(IOpenAIService sdk)
        {
            const string fileName = "SearchSample.json";

            try
            {
                Console.WriteLine($"Starting to read {fileName}");
                var searchSampleFile = await File.ReadAllBytesAsync($"SampleData/{fileName}");
                Console.WriteLine($"Uploading to read {fileName}");
                var uploadResult = await sdk.Files.FileUpload(UploadFilePurposes.UploadFilePurpose.Search.EnumToString(), searchSampleFile, fileName);
                if (uploadResult?.Successful == true)
                {
                    Console.WriteLine($"Uploading is done.");
                    Console.WriteLine($"File name:{uploadResult.FileName}");
                    Console.WriteLine($"File id:{uploadResult.Id}");
                    Console.WriteLine($"File purpose:{uploadResult.Purpose}");
                }

                Console.WriteLine($"Fetching files.");
                var uploadedFiles = await sdk.Files.FileList();
                var uploadedFile = uploadedFiles!.Data.Single(r => r.Id == uploadResult.Id);
                Console.WriteLine($"File found.");
                var file = await sdk.Files.FileRetrieve(uploadedFile.Id);
                Console.WriteLine($"File retrieved.{file.CreatedAt}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}