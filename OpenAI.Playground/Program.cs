using LaserCatEyes.DataServiceSdk;
using LaserCatEyes.Domain;
using LaserCatEyes.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Playground;
using OpenAI.SDK;
using OpenAI.SDK.Models.RequestModels;

var builder = new ConfigurationBuilder()
    .AddJsonFile("ApiSettings.json")
    .AddUserSecrets<Program>();

IConfiguration configuration = builder.Build();
var serviceCollection = new ServiceCollection();
serviceCollection.AddOptions<OpenAiSettings>();
serviceCollection.AddScoped(_ => configuration);

//I needed to create another Laser cat eyes message handler class, it is exactly same codes with original one but I am not sure yet why it wasn't working here
//Anyway, Laser cat eyes will help us to track request and responses betwen OpenAI server and our client
//It is in Beta version, if you have consider about your data privacy just comment out from here
serviceCollection.AddTransient<ILaserCatEyesDataService, LaserCatEyesDataService>();
serviceCollection.AddTransient<LaserCatEyesHttpMessageHandlerFIX>();
serviceCollection.Configure<LaserCatEyesOptions>(configuration.GetSection("LaserCatEyesOptions"));
serviceCollection.AddHttpClient<IOpenAISdk, OpenAISdk>().AddHttpMessageHandler<LaserCatEyesHttpMessageHandlerFIX>();
// to here, and un comment from here

//serviceCollection.AddHttpClient<IOpenAISdk, OpenAISdk>();

// to here

serviceCollection.Configure<OpenAiSettings>(configuration.GetSection(OpenAiSettings.SettingKey));

var serviceProvider = serviceCollection.BuildServiceProvider();

var sdk = serviceProvider.GetRequiredService<IOpenAISdk>();
var engineList = await sdk.ListEngines();

Console.WriteLine("Engine List:");
Console.WriteLine(string.Join(",", engineList.Engines.Select(r => r.Id)));

//foreach (var engineItem in engineList.Engines)
//{
//    Console.WriteLine($"Retrieve Engine:{engineItem.Id}");
//    var engine = await sdk.RetrieveEngine(engineItem.Id);
//    Console.WriteLine(engine.Status
//        ? $"Retrieved Engine:{engine.Id}"
//        : $"Couldn't Retrieve Engine:{engineItem.Id}");
//}

Console.WriteLine("Create completion:");
//TODO check why Laser Cat Eyes couldn't render response object
var completionResult = await sdk.CreateCompletion("davinci", new CreateCompletionRequest()
{
    Prompt = "Once upon a time",
    MaxTokens = 5
});
Console.WriteLine(completionResult.Choices.FirstOrDefault());
