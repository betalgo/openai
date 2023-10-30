using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.Playground.TestHelpers;
#if NET6_0_OR_GREATER
using LaserCatEyes.HttpClientListener;
#endif

var builder = new ConfigurationBuilder()
    .AddJsonFile("ApiSettings.json")
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddScoped(_ => configuration);

#if NET6_0_OR_GREATER
// Laser cat eyes is a tool that shows your requests and responses between OpenAI server and your client.
// Get your app key from https://lasercateyes.com for FREE and put it under ApiSettings.json or secrets.json.
// It is in Beta version, if you don't want to use it just comment out below line.
serviceCollection.AddLaserCatEyesHttpClientListener();
#endif

serviceCollection.AddOpenAIService();
//// DeploymentId and ResourceName are only for Azure OpenAI. If you want to use Azure OpenAI services you have to set Provider type To Azure.
//serviceCollection.AddOpenAIService(options =>
//{
//    options.ProviderType = ProviderType.Azure;
//    options.ApiKey = "Test";
//    options.DeploymentId = "MyDeploymentId";
//    options.ResourceName = "MyResourceName";
//});

var serviceProvider = serviceCollection.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<IOpenAIService>();

//                                 CHAT GPT
//  |-----------------------------------------------------------------------|
//  |    o   \ o /  _ o         __|    \ /     |__        o _  \ o /   o    |
//  |   /|\    |     /\   ___\o   \o    |    o/    o/__   /\     |    /|\   |
//  |   / \   / \   | \  /)  |    ( \  /o\  / )    |  (\  / |   / \   / \   |
//  |-----------------------------------------------------------------------|

await ChatCompletionTestHelper.RunSimpleChatCompletionTest(sdk);
//await ChatCompletionTestHelper.RunSimpleCompletionStreamTest(sdk);
//await ChatCompletionTestHelper.RunChatFunctionCallTest(sdk);
//await FineTuningJobTestHelper.RunCaseStudyIsTheModelMakingUntrueStatements(sdk);
// Whisper
//await AudioTestHelper.RunSimpleAudioCreateTranscriptionTest(sdk);
//await AudioTestHelper.RunSimpleAudioCreateTranslationTest(sdk);

//await ModelTestHelper.FetchModelsTest(sdk);
//await EditTestHelper.RunSimpleEditCreateTest(sdk);
//await ImageTestHelper.RunSimpleCreateImageTest(sdk);
//await ImageTestHelper.RunSimpleCreateImageEditTest(sdk);
//await ImageTestHelper.RunSimpleCreateImageVariationTest(sdk);
await ModerationTestHelper.CreateModerationTest(sdk);
//await CompletionTestHelper.RunSimpleCompletionTest(sdk);
//await CompletionTestHelper.RunSimpleCompletionTest2(sdk);
//await CompletionTestHelper.RunSimpleCompletionTest3(sdk);
//await CompletionTestHelper.RunSimpleCompletionStreamTest(sdk);
//await CompletionTestHelper.RunSimpleCompletionStreamTestWithCancellationToken(sdk);
//await CompletionTestHelper.RunSimpleCompletionTestWithCancellationToken(sdk);
//await EmbeddingTestHelper.RunSimpleEmbeddingTest(sdk);
//////await FileTestHelper.RunSimpleFileTest(sdk); //will delete all of your files
//////await FineTuningTestHelper.CleanUpAllFineTunings(sdk); //!!!!! will delete all fine-tunings
//await FineTuningTestHelper.RunCaseStudyIsTheModelMakingUntrueStatements(sdk);
await TokenizerTestHelper.RunTokenizerTest();
await TokenizerTestHelper.RunTokenizerCountTest();
await TokenizerTestHelper.RunTokenizerTestCrClean();

Console.ReadLine();