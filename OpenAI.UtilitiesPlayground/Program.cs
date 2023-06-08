using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.Utilities;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped(_ => configuration);


// Laser cat eyes is a tool that shows your requests and responses between OpenAI server and your client.
// Get your app key from https://lasercateyes.com for FREE and put it under ApiSettings.json or secrets.json.
// It is in Beta version, if you don't want to use it just comment out below line.
serviceCollection.AddLaserCatEyesHttpClientListener();


serviceCollection.AddOpenAIService();
var serviceProvider = serviceCollection.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<IOpenAIService>();


IEmbeddingTools embeddingTools = new EmbeddingTools(sdk,500,Models.TextEmbeddingAdaV2);
var dataFrame = await embeddingTools.ReadFilesAndCreateEmbeddingDataAsCsv(Path.Combine("Data", "OpenAI"),"processed/scraped.csv");
var dataFrame2 = embeddingTools.LoadEmbeddedDataFromCsv("processed/scraped.csv");

do
{
    Console.WriteLine("Ask a question:");
    var question = Console.ReadLine();
    if (question != null)
    {
        var context = embeddingTools.CreateContext(question, dataFrame);

        var completionResponse = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
        {
            Model = Models.Gpt_4,
            Messages = new List<ChatMessage>()
            {
                ChatMessage.FromSystem($"Answer the question based on the context below, and if the question can't be answered based on the context, say \"I don't know\".\n\nContext: {context}"),
                ChatMessage.FromUser(question)
            }
        });
        Console.WriteLine(completionResponse.Successful ? completionResponse.Choices.First().Message.Content : completionResponse.Error?.Message);
    }
} while (true);