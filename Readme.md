# Dotnet SDK for OpenAI ChatGPT, Whisper, GPT-4 and DALL·E

[![Betalgo.OpenAI](https://img.shields.io/nuget/v/Betalgo.OpenAI?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI/)

```
Install-Package Betalgo.OpenAI
```

Dotnet SDK for OpenAI Chat GPT, Whisper, GPT-4 ,GPT-3 and DALL·E  
*Unofficial*.  
*OpenAI doesn't have any official .Net SDK.*

#### This library used be to known as `Betalgo.OpenAI.GPT3`, now it has a new package Id `Betalgo.OpenAI`.

## Checkout the wiki page: 
https://github.com/betalgo/openai/wiki
## Checkout new ***experimantal*** utilities library:
[![Betalgo.OpenAI.Utilities](https://img.shields.io/nuget/v/Betalgo.OpenAI.Utilities?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.Utilities/)
```
Install-Package Betalgo.OpenAI.Utilities
```
Maintenance of this project is made possible by all the bug reporters, [contributors](https://github.com/betalgo/openai/graphs/contributors) and [sponsors](https://github.com/sponsors/kayhantolga).  
💖 Sponsors:  
[@betalgo](https://github.com/betalgo),
[Laser Cat Eyes](https://lasercateyes.com/)

[@oferavnery](https://github.com/oferavnery)
[@Removable](https://github.com/Removable)
## Features
- [X] [Function Calling](https://github.com/betalgo/openai/wiki/Function-Calling)
- [ ] Plugins (coming soon)
- [x] [Chat GPT](https://github.com/betalgo/openai/wiki/Chat-GPT)
- [x] [Chat GPT-4](https://github.com/betalgo/openai/wiki/Chat-GPT) *(models are supported, Image analyze API not released yet by OpenAI)*
- [x] [Azure OpenAI](https://github.com/betalgo/openai/wiki/Azure-OpenAI)
- [x] [Image DALL·E](https://github.com/betalgo/openai/wiki/Dall-E)
- [x] [Models](https://github.com/betalgo/openai/wiki/Models)
- [x] [Completions](https://github.com/betalgo/openai/wiki/Completions) 
- [x] [Edit](https://github.com/betalgo/openai/wiki/Edit) 
- [x] [Embeddings](https://github.com/betalgo/openai/wiki/Embeddings) 
- [x] [Files](https://github.com/betalgo/openai/wiki/Files) 
- [x] [Chatgpt Fine-Tuning](https://github.com/betalgo/openai/wiki/Chatgpt-Fine-Tuning) 
- [x] [Fine-tunes](https://github.com/betalgo/openai/wiki/Fine-Tuning)
- [x] [Moderation](https://github.com/betalgo/openai/wiki/Moderation)
- [x] [Tokenizer-GPT3](https://github.com/betalgo/openai/wiki/Tokenizer)
- [ ] [Tokenizer](https://github.com/betalgo/openai/wiki/Tokenizer)
- [x] [Whisper](https://github.com/betalgo/openai/wiki/Whisper)
- [x] [Rate limit](https://github.com/betalgo/openai/wiki/Rate-Limit)
- [x] [Proxy](https://github.com/betalgo/openai/wiki/Proxy)


For changelogs please go to end of the document.

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
## Function Sample
```csharp
var fn1 = new FunctionDefinitionBuilder("get_current_weather", "Get the current weather")
            .AddParameter("location", PropertyDefinition.DefineString("The city and state, e.g. San Francisco, CA"))
            .AddParameter("format", PropertyDefinition.DefineEnum(new List<string> { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .Validate()
            .Build();

        var fn2 = new FunctionDefinitionBuilder("get_n_day_weather_forecast", "Get an N-day weather forecast")
            .AddParameter("location", new() { Type = "string", Description = "The city and state, e.g. San Francisco, CA" })
            .AddParameter("format", PropertyDefinition.DefineEnum(new List<string> { "celsius", "fahrenheit" }, "The temperature unit to use. Infer this from the users location."))
            .AddParameter("num_days", PropertyDefinition.DefineInteger("The number of days to forecast"))
            .Validate()
            .Build();
        var fn3 = new FunctionDefinitionBuilder("get_current_datetime", "Get the current date and time, e.g. 'Saturday, June 24, 2023 6:14:14 PM'")
            .Build();

        var fn4 = new FunctionDefinitionBuilder("identify_number_sequence", "Get a sequence of numbers present in the user message")
            .AddParameter("values", PropertyDefinition.DefineArray(PropertyDefinition.DefineNumber("Sequence of numbers specified by the user")))
            .Build();

        ConsoleExtensions.WriteLine("Chat Function Call Test:", ConsoleColor.DarkCyan);
        var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous."),
                    ChatMessage.FromUser("Give me a weather report for Chicago, USA, for the next 5 days.")
                },
            Functions = new List<FunctionDefinition> { fn1, fn2, fn3, fn4 },
            MaxTokens = 50,
            Model = Models.Gpt_3_5_Turbo
        });

        if (completionResult.Successful)
        {
            var choice = completionResult.Choices.First();
            Console.WriteLine($"Message:        {choice.Message.Content}");

            var fn = choice.Message.FunctionCall;
            if (fn != null)
            {
                Console.WriteLine($"Function call:  {fn.Name}");
                foreach (var entry in fn.ParseArguments())
                {
                    Console.WriteLine($"  {entry.Key}: {entry.Value}");
                }
            }
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
### 7.2.0
- Added Chatgpt Finetununig support thanks to @aghimir3 
- Default Azure Openai version increased thanks to @mac8005
- Fixed Azure Openai Audio endpoint thanks to @mac8005
### 7.1.5
- Added error handling for PlatformNotSupportedException in PostAsStreamAsync when using HttpClient.Send, now falls back to SendRequestPreNet6 for compatibility on platforms like MAUI, Mac. Thanks to  @Almis90
- We now have a function caller describe method that automatically generates function descriptions. This method is available in the utilities library. Thanks to @vbandi
### 7.1.3
- This release was a bit late and took longer than expected due to a couple of reasons. The future was quite big, and I couldn't cover all possibilities. However, I believe I have covered most of the function definitions (with some details missing). Additionally, I added an option to build it manually. If you don't know what I mean, you don't need to worry. I plan to cover the rest of the function definition in the next release. Until then, you can discover this by playing in the playground or in the source code. This version also support using other libraries to export your function definition.
- We now have support for functions! Big cheers to @rzubek for completing most of this feature.
- Additionally, we have made bug fixes and improvements. Thanks to @choshinyoung, @yt3trees, @WeihanLi, @N0ker, and all the bug reporters. (Apologies if I missed any names. Please let me know if I missed your name and you have a commit.) 
### 7.1.2-beta
- Bugfix https://github.com/betalgo/openai/pull/302
- Added support for Function role https://github.com/betalgo/openai/issues/303
### 7.1.0-beta
- Function Calling: We're releasing this version to bring in a new feature that lets you call functions faster. But remember, this version might not be perfectly stable and we might change it a lot later. A big shout-out to @rzubek for helping us add this feature. Although I liked his work, I didn't have enough time to look into it thoroughly. Still, the tests I did showed it was working, so I decided to add his feature to our code. This lets everyone use it now. Even though I'm busy moving houses and didn't have much time, seeing @rzubek's help made things a lot easier for me.
- Support for New Models: This update also includes support for new models that OpenAI recently launched. I've also changed the naming style to match OpenAI's. Model names will no longer start with 'chat'; instead, they'll start with 'gpt_3_5' and so on.
### 7.0.0
- The code now supports .NET 7.0. Big cheers to @BroMarduk for making this happen.
- The library now automatically disposes of the Httpclient when it's created by the constructor. This feature is thanks to @BroMarduk.
- New support has been added for using more than one instance at the same time. Check out this [link](https://github.com/betalgo/openai/wiki/Working-with-Multiple-Instances) for more details. Thanks to @remixtedi for bringing this to my attention.
- A lot of small improvements have been done by @BroMarduk.
- **Breaking Changes** 😢
  - I've removed 'GPT3' from the namespace, so you might need to modify some aspects of your project. But don't worry, it's pretty simple! For instance, instead of writing `using OpenAI.GPT3.Interfaces`, you'll now write `using OpenAI.Interfaces`.
  - The order of the OpenAI constructor parameters has changed. It now takes 'options' first, then 'httpclient'.
    ```csharp
	//Before
	var openAiService = new OpenAIService(httpClient, options);
	//Now
	var openAiService = new OpenAIService(options, httpClient);
	```
