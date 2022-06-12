using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.Playground.TestHelpers;

var builder = new ConfigurationBuilder()
    .AddJsonFile("ApiSettings.json")
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped(_ => configuration);

// Laser cat eyes will help us to track request and responses between OpenAI server and our client
// It is in Beta version, if you have concerns about your data privacy or if you don't want to use it,
// just leave LaserCatEyesOptions:ApiKey empty in ApiSettings.json
//
// Get your app key from https://lasercateyes.com and put it under ApiSettings.json or secrets.json

if (!string.IsNullOrEmpty(configuration.GetValue<string>("LaserCatEyesOptions:ApiKey")))
{
    serviceCollection.AddLaserCatEyesHttpClientListener();
}

serviceCollection.AddOpenAIService();

//serviceCollection.AddOpenAIService(settings => { settings.ApiKey = "TEST"; });

var serviceProvider = serviceCollection.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<IOpenAIService>();

await ModelTestHelper.FetchModelsTest(sdk);
//await CompletionTestHelper.RunSimpleCompletionTest(sdk);
//await SearchTestHelper.SearchDocuments(sdk);
//await ClassificationsTestHelper.RunSimpleClassificationTest(sdk);
//await AnswerTestHelper.RunSimpleAnswerTest(sdk);
//await FileTestHelper.RunSimpleFileTest(sdk);
////await FineTuningTestHelper.CleanUpAllFineTunings(sdk); //!!!!! will delete all fine-tunings
//await FineTuningTestHelper.RunCaseStudyIsTheModelMakingUntrueStatements(sdk);
Console.ReadLine();