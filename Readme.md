# .NET SDK for OpenAI APIs

## .Net SDK for OpenAI, *Community Library*: 
[![Betalgo.OpenAI](https://img.shields.io/nuget/v/Betalgo.OpenAI?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI/)
```
Install-Package Betalgo.OpenAI
```


## ***experimental*** utilities library:
[![Betalgo.OpenAI.Utilities](https://img.shields.io/nuget/v/Betalgo.OpenAI.Utilities?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.Utilities/)
```
Install-Package Betalgo.OpenAI.Utilities
```

## Documentations and links: 
[Wiki Page](https://github.com/betalgo/openai/wiki)
[Feature Availability Table](https://github.com/betalgo/openai/wiki/Feature-Availability) 
[Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)

Betalgo.OpenAI: [![Static Badge](https://img.shields.io/badge/API%20Docs-DNDocs-190088?logo=readme&logoColor=white)](https://dndocs.com/d/betalgo-openai/api/OpenAI.OpenAiOptions.html)  
Betalgo.OpenAI.Utilities: [![Static Badge](https://img.shields.io/badge/API%20Docs-DNDocs-190088?logo=readme&logoColor=white)](https://dndocs.com/d/betalgo-openai/api/OpenAI.Utilities.Embedding.EmbeddingTools.html)

---

Maintenance of this project is made possible by all the bug reporters, [contributors](https://github.com/betalgo/openai/graphs/contributors) and [sponsors](https://github.com/sponsors/kayhantolga).  
💖 Sponsors:  
[@betalgo](https://github.com/betalgo),
[Laser Cat Eyes](https://lasercateyes.com/)

[@tylerje](https://github.com/tylerje) ,[@oferavnery](https://github.com/oferavnery), [@MayDay-wpf](https://github.com/MayDay-wpf), [@AnukarOP](https://github.com/AnukarOP), [@Removable](https://github.com/Removable)

---

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
    //,"UseBeta": "true/false (optional)"
  },
```
*(How to use [user secret](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows) ?  
Right click your project name in "solution explorer" then click "Manage User Secret", it is a good way to keep your api keys)*

### For Beta Features:
- Use `"UseBeta": true` in your config file or  `serviceCollection.AddOpenAIService(r=>r.UseBeta = true);` or `new OpenAiOptions { UseBeta = true }` in your service registration.


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

        var tools = new List<ToolDefinition>()
        {
            new ToolDefinition() { Function = fn1 },
            new ToolDefinition() { Function = fn2 },
            new ToolDefinition() { Function = fn3 },
            new ToolDefinition() { Function = fn4 },
        }

        ConsoleExtensions.WriteLine("Chat Function Call Test:", ConsoleColor.DarkCyan);
        var completionResult = await sdk.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("Don't make assumptions about what values to plug into functions. Ask for clarification if a user request is ambiguous."),
                    ChatMessage.FromUser("Give me a weather report for Chicago, USA, for the next 5 days.")
                },
            Tools = tools,
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

## VISION Sample
```csharp
var completionResult = await sdk.ChatCompletion.CreateCompletion(
    new ChatCompletionCreateRequest
    {
        Messages = new List<ChatMessage>
        {
            ChatMessage.FromSystem("You are an image analyzer assistant."),
            ChatMessage.FromUser(
                new List<MessageContent>
                {
                    MessageContent.TextContent("What is on the picture in details?"),
                    MessageContent.ImageUrlContent(
                        "https://www.digitaltrends.com/wp-content/uploads/2016/06/1024px-Bill_Cunningham_at_Fashion_Week_photographed_by_Jiyang_Chen.jpg?p=1",
                        ImageStatics.ImageDetailTypes.High
                    )
                }
            ),
        },
        MaxTokens = 300,
        Model = Models.Gpt_4_vision_preview,
        N = 1
    }
);

if (completionResult.Successful)
{
    Console.WriteLine(completionResult.Choices.First().Message.Content);
}
```

## VISION Sample using Base64 encoded image
```csharp
const string fileName = "image.png";
var binaryImage = await FileExtensions.ReadAllBytesAsync(fileName);

var completionResult = await sdk.ChatCompletion.CreateCompletion(
    new ChatCompletionCreateRequest
    {
        Messages = new List<ChatMessage>
        {
            ChatMessage.FromSystem("You are an image analyzer assistant."),
            ChatMessage.FromUser(
                new List<MessageContent>
                {
                    MessageContent.TextContent("What is on the picture in details?"),
                    MessageContent.ImageBinaryContent(
                        binaryImage,
                        ImageStatics.ImageFileTypes.Png,
                        ImageStatics.ImageDetailTypes.High
                    )
                }
            ),
        },
        MaxTokens = 300,
        Model = Models.Gpt_4_vision_preview,
        N = 1
    }
);

if (completionResult.Successful)
{
    Console.WriteLine(completionResult.Choices.First().Message.Content);
}
```

## Notes:
#### This library used to be known as `Betalgo.OpenAI.GPT3`, now it has a new package Id `Betalgo.OpenAI`.

Please note that due to time constraints, I was unable to thoroughly test all of the methods or fully document the library. If you encounter any issues, please do not hesitate to report them or submit a pull request - your contributions are always appreciated.

I initially developed this SDK for my personal use and later decided to share it with the community. As I have not maintained any open-source projects before, any assistance or feedback would be greatly appreciated. If you would like to contribute in any way, please feel free to reach out to me with your suggestions.

I will always be using the latest libraries, and future releases will frequently include breaking changes. Please take this into consideration before deciding to use the library. I want to make it clear that I cannot accept any responsibility for any damage caused by using the library. If you feel that this is not suitable for your purposes, you are free to explore alternative libraries or the OpenAI Web-API.

If I forgot your name in change logs, please accept my apologies and let me know so I can add it to the list.

## Changelog
### 8.2.0-beta
- Added support for beta features, such as assistants, threads, messages, and run. Still missing some of the endpoints, but good progress achieved. See complete list from here: [Feature Availability Table](https://github.com/betalgo/openai/wiki/Feature-Availability). Thanks to @CongquanHu , @alistein, @hucongquan.
- Use `"UseBeta": true` in your config file or  `serviceCollection.AddOpenAIService(r=>r.UseBeta = true);` or `new OpenAiOptions { UseBeta = true }` in your service registration.

### 8.1.1
- Fixed incorrect mapping for batch API error response.
### 8.1.0
- Added support for Batch API
### 8.0.1
- Added support for new Models `gpt-4-turbo` and `gpt-4-turbo-2024-04-09` thanks to @ChaseIngersol
### 8.0.0
- Added support for .NET 8.0 thanks to @BroMarduk
- Utilities library updated to work with only .NET 8.0
### 7.4.7
- Fixed a bug that was causing binary image to be sent as base64 string, Thanks to @yt3trees
- Fixed a bug that was blocking CreateCompletionAsStream on some platforms. #331
- Fixed a bug that was causing an error with multiple tool calls, now we are handling index parameter #493, thanks to @David-Buyer
