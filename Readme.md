# Dotnet SDK for OpenAI GPT-3 and DALL·E

[![Betalgo.OpenAI.GPT3](https://img.shields.io/nuget/v/Betalgo.OpenAI.GPT3?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.GPT3/)

```
Install-Package Betalgo.OpenAI.GPT3
```

Dotnet SDK for OpenAI GPT-3 and DALL·E  
*Unofficial*.  
*GPT-3 doesn't have any official .Net SDK.*
## Checkout the wiki page: 
https://github.com/betalgo/openai/wiki
## Features
- [ ] ChatGPT (coming soon)
- [x] Image (DALL·E)
- [x] Models
- [x] Completions
- [x] Edit
- [x] Mars
- [x] Embeddings
- [x] Files
- [x] Fine-tunes
- [x] Moderation
- [ ] Rate limit support

For changelogs please go to end of the document.

Visit https://openai.com/ to get your API key. Also documentation with more detail is avaliable there.  



## Sample Usages
### ***!! I would strongly suggest to use different account than your main account while you use playground.   Test methods could add or delete your files and models !!***

The repository includes one sample project already **"OpenAI.Playground"** You can check playground project to see how I was testing it while I was developing the library. Be careful while playing with it. Some test methods will delete your files or fine tunings.  

Your API Key comes from here --> https://platform.openai.com/account/api-keys

Your Organization ID comes from here --> https://platform.openai.com/account/org-settings

### Without using dependency injection:
```csharp
var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey =  Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY")
});
```
### Using dependency injection:
#### secrets.json: 

```csharp
 "OpenAIServiceOptions": {
    //"ApiKey":"Your api key goes here"
    //,"Organization": "Your Organization Id goes here (optional)"
  },
```
*(How to use [user secret](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) ?  
Right click your project name in "solution explorer" then click "Manage User Secret", it is a good way to keep your api keys)*

#### Program.cs
```csharp
serviceCollection.AddOpenAIService();
```

**OR**  
Use it like below but do NOT put your API key directly to your source code. 
#### Program.cs
```csharp
serviceCollection.AddOpenAIService(settings => { settings.ApiKey = Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY"); });
```

After injecting your service you will be able to get it from service provider
```csharp
var openAiService = serviceProvider.GetRequiredService<IOpenAIService>();
```

You can set default model(optional):
```csharp
openAiService.SetDefaultModelId(Models.Davinci);
```

## Completions Sample
```csharp
var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
{
    Prompt = "Once upon a time",
    Model = Models.TextDavinciV3
});

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

## Completions Stream Sample
```csharp
var completionResult = sdk.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
   {
      Prompt = "Once upon a time",
      MaxTokens = 50
   }, Models.Davinci);

   await foreach (var completion in completionResult)
   {
      if (completion.Successful)
      {
         Console.Write(completion.Choices.FirstOrDefault()?.Text);
      }
      else
      {
         if (completion.Error == null)
         {
            throw new Exception("Unknown Error");
         }

         Console.WriteLine($"{completion.Error.Code}: {completion.Error.Message}");
      }
   }
   Console.WriteLine("Complete");
```

## DALL·E Sample
```csharp
var imageResult = await sdk.Image.CreateImage(new ImageCreateRequest
{
    Prompt = "Laser cat eyes",
    N = 2,
    Size = StaticValues.ImageStatics.Size.Size256,
    ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
    User = "TestUser"
});


if (imageResult.Successful)
{
    Console.WriteLine(string.Join("\n", imageResult.Results.Select(r => r.Url)));
}
```

## Notes:
I couldn't find enough time to test all the methods or improve the documentation. My main target was to make fine-tuning available. If you hit any issue please report it or pull request always appreciated. 

*I was building an SDK for myself then I decide to share it, I hope it will be useful for you. I haven't maintained any open source projects before. Any help would be much appreciated. I am open to suggestions If you would like to contribute somehow.*

I will be using the latest libraries all the time. Also, next releasing will include breaking changes frequently *(as I mentioned before I was building the SDK for myself. Unfortunately I do not have time to plan these changes and support lower version apps)*. So please be aware of that before starting to use the library. 

As you can guess I do not accept any damage caused by use of the library. You are always free to use other libraries or OpenAI Web-API.



## Changelog
### 6.6.7
* Added Cancellation Token support, thanks to @robertlyson 
* Updated readme file, thanks to @qbm5, @gotmike, @SteveMCarroll
