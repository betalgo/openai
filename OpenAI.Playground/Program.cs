using LaserCatEyes.Domain.Models;
using LaserCatEyes.HttpClientListener;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Playground;
using OpenAI.SDK;
using OpenAI.SDK.Interfaces;

var builder = new ConfigurationBuilder()
    .AddJsonFile("ApiSettings.json")
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddOptions<OpenAiSettings>();
serviceCollection.AddScoped(_ => configuration);


// Laser cat eyes will help us to track request and responses between OpenAI server and our client
//It is in Beta version, if you have consider about your data privacy or if you don't want to use it just comment out from here
serviceCollection.Configure<LaserCatEyesOptions>(configuration.GetSection("LaserCatEyesOptions"));
serviceCollection.AddHttpClient<IOpenAISdk, OpenAISdk>();
serviceCollection.AddLaserCatEyesHttpClientListener();

//// to here, and uncomment from here
//serviceCollection.AddHttpClient<IOpenAISdk, OpenAISdk>();
//// to here

serviceCollection.Configure<OpenAiSettings>(configuration.GetSection(OpenAiSettings.SettingKey));
var serviceProvider = serviceCollection.BuildServiceProvider();
var sdk = serviceProvider.GetRequiredService<IOpenAISdk>();
var engineList = await sdk.Engine.ListEngines();
//Console.WriteLine("Engine List:");
//if (engineList == null)
//{
//    throw new NullReferenceException(nameof(engineList));
//}
//Console.WriteLine(string.Join(",", engineList.Engines.Select(r => r.Id)));

////foreach (var engineItem in engineList.Engines)
////{
////    Console.WriteLine($"Retrieve Engine:{engineItem.Id}");
////    var engine = await sdk.RetrieveEngine(engineItem.Id);
////    Console.WriteLine(engine.Status
////        ? $"Retrieved Engine:{engine.Id}"
////        : $"Couldn't Retrieve Engine:{engineItem.Id}");
////}

//Console.WriteLine("Create completion:");
////TODO check why Laser Cat Eyes couldn't render response object
//var completionResult = await sdk.Completions.CreateCompletion("davinci", new CreateCompletionRequest()
//{
//    Prompt = "Once upon a time",
//    MaxTokens = 5
//});
//Console.WriteLine(completionResult.Choices.FirstOrDefault());
//var fileName = "AnswerQuestionsSample.json";
//var searchSampleFile = await File.ReadAllBytesAsync($"SampleData/{fileName}");
//await sdk.Files.UploadFiles(UploadFilePurpose.Answers, searchSampleFile, fileName);
//var uploadedFiles = await sdk.Files.ListFiles();
//foreach (var uploadedFile in uploadedFiles.Data)
//{
//    Console.WriteLine(uploadedFile.FileName);
//    var file = await sdk.Files.RetrieveFile(uploadedFile.Id);
//    Console.WriteLine(file.FileName);
//    //   var fileContent = sdk.Files.RetrieveFileContent(file.Id);
//    var deleteResponse = await sdk.Files.DeleteFile(file.Id);
//}
await FileTestHelper.CleanAllFiles(sdk);
await SearchTestHelper.UploadSampleFileAndGetSearchResponse(sdk);