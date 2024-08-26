using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.UtilitiesPlayground.TestHelpers;

var builder = new ConfigurationBuilder().AddUserSecrets<Program>();

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


//await EmbeddingTestHelpers.ExerciseEmbeddingTools(sdk);
//await FunctionCallingTestHelpers.ExerciseFunctionCalling(sdk);
await JsonSchemaResponseTypeTestHelpers.RunChatWithJsonSchemaResponseFormat2(sdk);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();