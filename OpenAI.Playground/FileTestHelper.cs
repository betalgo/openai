using OpenAI.SDK.Interfaces;

namespace OpenAI.Playground
{
    internal static class FileTestHelper
    {
        public static async Task CleanAllFiles(IOpenAISdk sdk)
        {
            try
            {
                Console.WriteLine($"Starting to clean All Files");
                var uploadedFiles = await sdk.Files.ListFiles();
                foreach (var uploadedFile in uploadedFiles!.Data)
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
}