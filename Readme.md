# Dotnet SDK for OpenAI GTP-3

[![Betalgo.OpenAI.GPT3](https://img.shields.io/nuget/v/Betalgo.OpenAI.GPT3?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.GPT3/)

```
Install-Package Betalgo.OpenAI.GPT3
```
For changelogs please go to end of the document.

Unofficial - Dotnet SDK for OpenAI GTP-3. 
GPT-3 doesn't have any official .Net SDK.

## Features
- [x] Models
- [x] Completions
- [ ] Edit
- [x] Mars
- [x] Embeddings
- [x] Files
- [x] Fine-tunes
- [ ] Moderation


*I was building an SDK for myself then I decide to share it, I hope it will be useful for you. I haven't maintained any open source projects before. Any help would be much appreciated. I am open to suggestions If you would like to contribute somehow.*

I will be using the latest libraries all the time. Also, next releasing will include breaking changes frequently *(as I mentioned before I was building the SDK for myself. Unfortunately I do not have time to plan these changes and support lower version apps)*. So please be aware of that before starting to use the library. 

As you can guess I do not accept any damage caused by use of the library. You are always free to use other libraries or OpenAI Web-API.

Visit https://openai.com/ to get your API key. Also documentation with more detail is avaliable there.

## Sample Usages
The repository includes one sample project already **"OpenAI.Playground"** You can check playground project to see how I was testing it while I was developing the library. Be carefull while playing with it. Some test methods will delete your files or fine tunings. **I would suggest to use different account than your main account while you use playground.**

### Using dependcy injection:
#### secrets.json: 
*(How to use [user secret](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows)? => right click your project name in "solution explorer" then click "Manage User Secret", it is a good way to keep your api keys) *
```csharp
 "OpenAIServiceOptions": {
    //"ApiKey":"Your api key goes here"
    //,"Organization": "Your Organization Id goes here (optional)"
  },
```
#### Program.cs
```csharp
serviceCollection.AddOpenAIService();
```

or use it like below but do NOT put your API key directly to your source code. 
#### Program.cs
```csharp
serviceCollection.AddOpenAIService(settings => { settings.ApiKey = Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY"); });
```

After injecting your service you will be able to get it from service provider
```csharp
var openAiService = serviceProvider.GetRequiredService<IOpenAIService>();
```

Without dependcy injection:
```csharp
var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey =  Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY")
});
```

You can set default engine(optional):
```csharp
openAiService.SetDefaultEngineId(Engines.Davinci);
```

## Completions Sample
```csharp
var completionResult = await openAiService.Completions.Create(new CompletionCreateRequest()
{
  Prompt = "Once upon a time",
  MaxTokens = 5
}, Engines.Engine.Davinci);

if (completionResult.Successful)
{
    Console.WriteLine(completionResult.Choices.FirstOrDefault());
} 
else
{
  if (completionResult.Error == null)
  {
    throw new Exception("Unknown Error");
  }
  Console.WriteLine($"{completionResult.Error.Code}: {completionResult.Error.Message}");
 }
```


Notes:
I couldn't find enough time to test all the methods or improve the documentation. My main target was to make fine-tuning available. If you hit any issue please report it or pull request always appreciated. 


## Changelog
### 6.1.0
* Organization id is not a required value anymore, Thanks to @samuelnygaard
* Removed deprecated Engine Endpoint and replaced it with Models Endpoint. Now Model response have more fields.
* Regarding OpenAI Engine naming, I had to rename Engine Enum and static fields. They are quite similar but you have to replace them with new ones. Please use Models class instead of Engine class.
* To support fast engine name changing I have created a new Method, `Models.ModelNameBuilder()` you may consider using it.
### 6.2.0
* Removed deprecated Answers, Classifications, and Search endpoints https://community.openai.com/t/answers-classification-search-endpoint-deprecation/18532. They will be still avaliable until December at web-API. If you still need them please do not update to this version.
* Code clean-up
### 6.3.0
* Thanks to @c-d and @sarilouis for contributions on this version.
* Now we support Embedding endpoint. Thanks to @sarilouis
* Bug fixes and updates for Models
* Code clean-up