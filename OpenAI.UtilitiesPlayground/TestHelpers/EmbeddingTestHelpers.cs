using Betalgo.OpenAI.Utilities.Embedding;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;

namespace OpenAI.UtilitiesPlayground.TestHelpers;

public static class EmbeddingTestHelpers
{
    public static async Task ExerciseEmbeddingTools(IOpenAIService openAIService)
    {
        IEmbeddingTools embeddingTools = new EmbeddingTools(openAIService, 500, Models.TextEmbeddingAdaV2);

        var dataFrame = await embeddingTools.ReadFilesAndCreateEmbeddingDataAsCsv(Path.Combine("Data", "OpenAI"), "processed/scraped.csv");

        var dataFrame2 = embeddingTools.LoadEmbeddedDataFromCsv("processed/scraped.csv");

        do
        {
            Console.WriteLine("Ask a question:");
            var question = Console.ReadLine();

            if (question != null)
            {
                var context = embeddingTools.CreateContext(question, dataFrame);

                var completionResponse = await openAIService.ChatCompletion.CreateCompletion(new()
                {
                    Model = Models.Gpt_4,
                    Messages = new List<ChatMessage>
                    {
                        ChatMessage.FromSystem($"Answer the question based on the context below, and if the question can't be answered based on the context, say \"I don't know\".\n\nContext: {context}"),
                        ChatMessage.FromUser(question)
                    }
                });

                Console.WriteLine(completionResponse.Successful ? completionResponse.Choices.First().Message.Content : completionResponse.Error?.Message);
            }
        } while (true);
    }
}