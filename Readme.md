# Dotnet SDK for OpenAI ChatGPT, Whisper, GPT-4 and DALL·E

[![Betalgo.OpenAI.GPT3](https://img.shields.io/nuget/v/Betalgo.OpenAI.GPT3?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.GPT3/)

```
Install-Package Betalgo.OpenAI.GPT3
```

Dotnet SDK for OpenAI Chat GPT, Whisper, GPT-4 ,GPT-3 and DALL·E  
*Unofficial*.  
*OpenAI doesn't have any official .Net SDK.*
## Checkout the wiki page: 
https://github.com/betalgo/openai/wiki

## Features
- [x] [Chat GPT](https://github.com/betalgo/openai/wiki/Chat-GPT)
- [x] Azure OpenAI Support
- [x] [Image DALL·E](https://github.com/betalgo/openai/wiki/Dall-E)
- [x] [Models](https://github.com/betalgo/openai/wiki/Models)
- [x] [Completions](https://github.com/betalgo/openai/wiki/Completions) 
- [x] [Edit](https://github.com/betalgo/openai/wiki/Edit) 
- [x] [Embeddings](https://github.com/betalgo/openai/wiki/Embeddings) 
- [x] [Files](https://github.com/betalgo/openai/wiki/Files) 
- [x] [Fine-tunes](https://github.com/betalgo/openai/wiki/Fine-Tuning) 
- [x] [Moderation](https://github.com/betalgo/openai/wiki/Moderation)
- [x] Tokenizer Support
- [x] Whisper
- [ ] Rate limit support
- [ ] Chat GPT-4 support (models are supported, Image analyze API not released yet by OpenAI)

For changelogs please go to end of the document.

Visit https://openai.com/ to get your API key. Also documentation with more detail is avaliable there.  

## Sample Usages
The repository contains a sample project named **OpenAI.Playground** that you can refer to for a better understanding of how the library works. However, please exercise caution while experimenting with it, as some of the test methods may result in unintended consequences such as file deletion or fine tuning.


*!! It is highly recommended that you use a separate account instead of your primary account while using the playground. This is because some test methods may add or delete your files and models, which could potentially cause unwanted issues. !!*

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
## Chat Gpt Sample
```csharp
var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
{
    Messages = new List<ChatMessage>
    {
        ChatMessage.FromSystem("You are a helpful assistant."),
        ChatMessage.FromUser("Who won the world series in 2020?"),
        ChatMessage.FromAssistant("The Los Angeles Dodgers won the World Series in 2020."),
        ChatMessage.FromUser("Where was it played?")
    },
    Model = Models.ChatGpt3_5Turbo,
    MaxTokens = 50//optional
});
if (completionResult.Successful)
{
   Console.WriteLine(completionResult.Choices.First().Message.Content);
}
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
var completionResult = openAiService.Completions.CreateCompletionAsStream(new CompletionCreateRequest()
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
var imageResult = await openAiService.Image.CreateImage(new ImageCreateRequest
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
Please note that due to time constraints, I was unable to thoroughly test all of the methods or fully document the library. If you encounter any issues, please do not hesitate to report them or submit a pull request - your contributions are always appreciated.

I initially developed this SDK for my personal use and later decided to share it with the community. As I have not maintained any open-source projects before, any assistance or feedback would be greatly appreciated. If you would like to contribute in any way, please feel free to reach out to me with your suggestions.

I will always be using the latest libraries, and future releases will frequently include breaking changes. Please take this into consideration before deciding to use the library. I want to make it clear that I cannot accept any responsibility for any damage caused by using the library. If you feel that this is not suitable for your purposes, you are free to explore alternative libraries or the OpenAI Web-API.

I am incredibly busy. If I forgot your name, please accept my apologies and let me know so I can add it to the list.

## Changelog
### 6.8.3
- **Breaking Changes**: 
    - ~~I am going to update library namespace from `Betalgo.OpenAI.GPT3` to `OpenAI.GPT3`. This is the first time I am trying to update my nuget packageId. If something broken, please be patient. I will be fixing it soon.~~
    Reverted namespace change, maybe next time.
    - Small Typo change on model name `Model.GPT4` `to Model.GPT_4`

    - `ServiceCollection.AddOpenAIService();` now returns `IHttpClientBuilder` which means it allows you to play with httpclient object. Thanks for all the reporters and @LGinC.
    Here is a little sample
```csharp
ServiceCollection.AddOpenAIService()
.ConfigurePrimaryHttpMessageHandler((s => new HttpClientHandler
{
    Proxy = new WebProxy("1.1.1.1:1010"),
});
``` 
### 6.8.1
- **Breaking Changes**: Typo fixed in Content Moderation CategoryScores, changing `Sexualminors` to `SexualMinors`. Thanks to @HowToDoThis.
- Tokenizer changes thanks to @IS4Code.
    - Performance improvement
    - Introduced a new method `TokenCount` that returns the number of tokens instead of a list.
    - **Breaking Changes**: Removed overridden methods that were basically string conversions. 
    I think these methods were not used much and it is fairly easy to do these conversions outside of the method. 
    If you disagree, let me know and I can consider adding them back.
### 6.8.0
* Added .Net Standart Support, Massive thanks to @pdcruze and @ricaun

### 6.7.3
* **Breaking change**: `ChatMessage.FromAssistance` is now `ChatMessage.FromAssistant`. Thanks to @Swimburger 
* The Tokenizer method has been extended with `cleanUpCREOL`. You can use this option to clean up Windows-style line endings. Thanks to @gspentzas1991 

### 6.7.2
* Removed Microsoft.AspNet.WebApi.Client dependecy
* The action build device has been updated to ubuntu due to suspicions that the EOL of the vocab.bpe file had been altered in the last few Windows builds.
* Added support for TextEmbeddingAdaV2 Model.