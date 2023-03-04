# Dotnet SDK for OpenAI Chat GPT, GPT-3 and DALL·E

[![Betalgo.OpenAI.GPT3](https://img.shields.io/nuget/v/Betalgo.OpenAI.GPT3?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.GPT3/)

```
Install-Package Betalgo.OpenAI.GPT3
```

Dotnet SDK for OpenAI Chat GPT, GPT-3 and DALL·E  
*Unofficial*.  
*GPT-3 doesn't have any official .Net SDK.*
## Checkout the wiki page: 
https://github.com/betalgo/openai/wiki

## **NOTE**  for v6.7.0 & v6.7.1
I know we are all excited about new Chat Gpt APIs, so I tried to rush this version. It's nearly 4 AM here.  
Be aware! It might have some bugs, also the next version may have breaking changes. Because I didn't like namings but I don't have time to think about it at the moment.  Whisper is coming soon to.

Enjoy your new Methods! Don't forget to star the repo if you like it.

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

For changelogs please go to end of the document.

Visit https://openai.com/ to get your API key. Also documentation with more detail is avaliable there.  



## Sample Usages
The repository contains a sample project named **OpenAI.Playground** that you can refer to for a better understanding of how the library works. However, please exercise caution while experimenting with it, as some of the test methods may result in unintended consequences such as file deletion or fine tuning.


### ***!! It is highly recommended that you use a separate account instead of your primary account while using the playground. This is because some test methods may add or delete your files and models, which could potentially cause unwanted issues. !!***

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
var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
{
    Messages = new List<ChatMessage>
    {
        ChatMessage.FromSystem("You are a helpful assistant."),
        ChatMessage.FromUser("Who won the world series in 2020?"),
        ChatMessage.FromAssistance("The Los Angeles Dodgers won the World Series in 2020."),
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
Please note that due to time constraints, I was unable to thoroughly test all of the methods or fully document the library. If you encounter any issues, please do not hesitate to report them or submit a pull request - your contributions are always appreciated.

I initially developed this SDK for my personal use and later decided to share it with the community. As I have not maintained any open-source projects before, any assistance or feedback would be greatly appreciated. If you would like to contribute in any way, please feel free to reach out to me with your suggestions.

I will always be using the latest libraries, and future releases will frequently include breaking changes. Please take this into consideration before deciding to use the library. I want to make it clear that I cannot accept any responsibility for any damage caused by using the library. If you feel that this is not suitable for your purposes, you are free to explore alternative libraries or the OpenAI Web-API.


## Changelog
### 6.7.1
* Introduced support for Whisper.
* Grateful thanks to @shanepowell for contributing RetrieveFileContent.
* Resolved an issue that was causing problems with the tokenizer. A clean build should hopefully address this.

### 6.7.0
* We all beeen waiting for this moment. Please enjoy Chat GPT API
* Added support for Chat GPT API
* Fixed Tokenizer Bug, it was not working properly.

### 6.6.8
* **Breaking Changes**
    * Renamed `Engine` keyword to `Model` in accordance with OpenAI's new naming convention.
    * Deprecated `DefaultEngineId` in favor of `DefaultModelId`.
    * `DefaultEngineId` and `DefaultModelId` is not static anymore.

* Added support for Azure OpenAI, a big thanks to @copypastedeveloper!
* Added support for Tokenizer, inspired by @dluc's https://github.com/dluc/openai-tools repository. Please consider giving the repo a star.  

These two changes are recent additions, so please let me know if you encounter any issues.
* Updated documentation links from beta.openai.com to platform.openai.com.
