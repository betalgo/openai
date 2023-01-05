# Dotnet SDK for OpenAI GPT-3 and DALL·E

[![Betalgo.OpenAI.GPT3](https://img.shields.io/nuget/v/Betalgo.OpenAI.GPT3?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.GPT3/)

```
Install-Package Betalgo.OpenAI.GPT3
```

Dotnet SDK for OpenAI GPT-3 and DALL·E  
*Unofficial*.  
*GPT-3 doesn't have any official .Net SDK.*

## Features
- [x] Image (DALL·E)
- [x] Models
- [x] Completions
- [x] Edit
- [x] Mars
- [x] Embeddings
- [x] Files
- [x] Fine-tunes
- [x] Moderation

For changelogs please go to end of the document.

Visit https://openai.com/ to get your API key. Also documentation with more detail is avaliable there.

## Sample Usages
### ***!! I would strongly suggest to use different account than your main account while you use playground.   Test methods could add or delete your files and models !!***

The repository includes one sample project already **"OpenAI.Playground"** You can check playground project to see how I was testing it while I was developing the library. Be carefull while playing with it. Some test methods will delete your files or fine tunings.  


### Without using dependcy injection:
```csharp
var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey =  Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY")
});
```
### Using dependcy injection:
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



You can set default engine(optional):
```csharp
openAiService.SetDefaultEngineId(Engines.Davinci);
```

## Completions Sample
```csharp
var completionResult = await openAiService.Completions.CreateCompletion(new CompletionCreateRequest()
{
    Prompt = "Once upon a time",
    MaxTokens = 5
}, Models.Davinci);

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


## Notes:
I couldn't find enough time to test all the methods or improve the documentation. My main target was to make fine-tuning available. If you hit any issue please report it or pull request always appreciated. 

*I was building an SDK for myself then I decide to share it, I hope it will be useful for you. I haven't maintained any open source projects before. Any help would be much appreciated. I am open to suggestions If you would like to contribute somehow.*

I will be using the latest libraries all the time. Also, next releasing will include breaking changes frequently *(as I mentioned before I was building the SDK for myself. Unfortunately I do not have time to plan these changes and support lower version apps)*. So please be aware of that before starting to use the library. 

As you can guess I do not accept any damage caused by use of the library. You are always free to use other libraries or OpenAI Web-API.



## Changelog
### 6.6.3
* Bug-fix, now we are handling logprops response properly, thanks to @KosmonikOS
* Code clean-up, thanks to @KosmonikOS

### 6.6.2
* Bug-fix,added jsonignore for `stop` and `stopAsList`, thanks to @Patapum 
    
### 6.6.1
* **Breaking change**. 
    * `EmbeddingCreateRequest.Input` was a ***string list*** type now it is a ***string*** type.  
    I have introduced `InputAsList` property instead of `Input`. You may need to update your code according the change.  
    ***Both Input(string) and InputAsList(string list) avaliable for use***

* Added string and string List support for some of the propertis.
    * CompletionCreateRequest --> Prompt & PromptAsList / Stop & StopAsList 
    * CreateModerationRequest --> Input & InputAsList 
    * EmbeddingCreateRequest --> Input & InputAsList
    
### 6.6.0
* Added support for new models (davinciv3 & edit models)
* Added support for Edit endpoint.
* (*Warning*: edit endpoint works with only some of the models, I couldn't find documentation about it, please follow the thread for more information: https://community.openai.com/t/is-edit-endpoint-documentation-incorrect/23361 )
* Some objects were created as class instead of record at last version. I change them to record. This will be breaking changes for some of you.
* With this version I think we cover all of openai APIs 
* In next version I will be focusing on code cleanup and refactoring. 
* If I don't need to relase bug-fix for this version also I will be updating library with dotnet 7 in next version as I promised.
### 6.5.0
* OpenAI made a surprise release yesterday and they have announced DALL·E API. I needed to do other things but I couldn't resist. Because I was rushing, some methods and class names may will change in the next release. Until that day, enjoy your creative AI.  
* **This library now fully support all DALL·E features**.
* I tried to complete Edit API too bu unfortunately something was wrong with the documentation, I need to ask some questions in the community forum.
### 6.4.1
* Bug-fixes 
    * FineTuneCreateRequest suffix json property name changed "Suffix" to "suffix"
    * CompletionCreateRequest user json property name changed "User" to "user" (Thanks to @shaneqld), also now it is a nullable string 
### 6.4.0
* I have good news and bad news
* Moderation feature implementation is done. Now we support Moderation.
* Updated some request and response models to catch up with changes in OpenAI API
* New version has some breaking changes. Because we are in the fall season I needed to do some cleanup. Sorry for breaking changes but most of them are just renaming. I believe they can be solved before your coffee finish.
* I am hoping to support Edit Feature in the next version.
### 6.3.0
* Thanks to @c-d and @sarilouis for their contributions to this version.
* Now we support Embedding endpoint. Thanks to @sarilouis
* Bug fixes and updates for Models
* Code clean-up
### 6.2.0
* Removed deprecated Answers, Classifications, and Search endpoints https://community.openai.com/t/answers-classification-search-endpoint-deprecation/18532. They will be still available until December at web-API. If you still need them please do not update to this version.
* Code clean-up
### 6.1.0
* Organization id is not a required value anymore, Thanks to @samuelnygaard
* Removed deprecated Engine Endpoint and replaced it with Models Endpoint. Now Model response has more fields.
* Regarding OpenAI Engine naming, I had to rename Engine Enum and static fields. They are quite similar but you have to replace them with new ones. Please use Models class instead of Engine class.
* To support fast engine name changing I have created a new Method, `Models.ModelNameBuilder()` you may consider using it.
