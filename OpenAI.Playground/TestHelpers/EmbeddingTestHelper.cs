using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.ObjectModels;
using OpenAI.Playground.ExtensionsAndHelpers;

namespace OpenAI.Playground.TestHelpers;

internal static class EmbeddingTestHelper
{
    public static async Task RunSimpleEmbeddingTest(IOpenAIService sdk)
    {
        ConsoleExtensions.WriteLine("Simple Embedding test is starting:", ConsoleColor.Cyan);

        try
        {
            ConsoleExtensions.WriteLine("Embedding Test:", ConsoleColor.DarkCyan);
            var embeddingResult = await sdk.Embeddings.CreateEmbedding(new()
            {
                InputAsList = new() { "The quick brown fox jumped over the lazy dog." },
                Model = Models.TextEmbeddingAdaV2
            });

            if (embeddingResult.Successful)
            {
                Console.WriteLine(embeddingResult.Data.FirstOrDefault());
            }
            else
            {
                if (embeddingResult.Error == null)
                {
                    throw new("Unknown Error");
                }

                Console.WriteLine($"{embeddingResult.Error.Code}: {embeddingResult.Error.Message}");
            }

            Console.WriteLine(embeddingResult.Data.FirstOrDefault());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}