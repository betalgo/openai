
# .NET SDK for OpenAI
⭐ We appreciate your star, it helps!  
 
[![Discord Shield](https://discord.com/api/guilds/1250841506785529916/widget.png?style=shield)](https://discord.gg/rE9uVp52) *(If the invite link doesn't work, ping me in discussions.)*  
We have a very new Discord channel. Please come and help us build the .NET AI community.

## Overview
A .NET SDK for accessing OpenAI's API, provided as a community library. This SDK allows you to integrate OpenAI's capabilities into your .NET applications with ease.

### Install Packages
#### Core Library
[![Betalgo.OpenAI](https://img.shields.io/nuget/v/Betalgo.OpenAI?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI/)
```shell
Install-Package Betalgo.OpenAI
```

#### Experimental Utilities Library
[![Betalgo.OpenAI.Utilities](https://img.shields.io/nuget/v/Betalgo.OpenAI.Utilities?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.Utilities/)
```shell
Install-Package Betalgo.OpenAI.Utilities
```

## Documentation and Links
- [Wiki Page](https://github.com/betalgo/openai/wiki)
- [Feature Availability Table](https://github.com/betalgo/openai/wiki/Feature-Availability)
- [Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)

### API Documentation
- Betalgo.OpenAI: [![Static Badge](https://img.shields.io/badge/API%20Docs-DNDocs-190088?logo=readme&logoColor=white)](https://dndocs.com/d/betalgo-openai/api/OpenAI.OpenAiOptions.html)
- Betalgo.OpenAI.Utilities: [![Static Badge](https://img.shields.io/badge/API%20Docs-DNDocs-190088?logo=readme&logoColor=white)](https://dndocs.com/d/betalgo-openai/api/OpenAI.Utilities.Embedding.EmbeddingTools.html)

---

## Acknowledgements
Maintenance of this project is made possible by all the bug reporters, [contributors](https://github.com/betalgo/openai/graphs/contributors), and [sponsors](https://github.com/sponsors/kayhantolga).

💖 Sponsors:  
[@betalgo](https://github.com/betalgo), [Laser Cat Eyes](https://lasercateyes.com/)   
[@tylerje](https://github.com/tylerje), [@oferavnery](https://github.com/oferavnery), [@MayDay-wpf](https://github.com/MayDay-wpf), [@AnukarOP](https://github.com/AnukarOP), [@Removable](https://github.com/Removable), [@Scar11](https://github.com/Scar11)

---

## Sample Usage
The repository contains a sample project named **OpenAI.Playground** to help you understand how the library works. However, please exercise caution while experimenting, as some test methods may result in unintended consequences such as file deletion or fine-tuning.

*!! It is highly recommended that you use a separate account instead of your primary account while using the playground. Some test methods may add or delete your files and models, potentially causing unwanted issues. !!*

Your API Key can be obtained from here: https://platform.openai.com/account/api-keys  
Your Organization ID can be found here: https://platform.openai.com/account/org-settings

### Without Using Dependency Injection
```csharp
var openAiService = new OpenAIService(new OpenAiOptions()
{
    ApiKey = Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY")
});
```

### Using Dependency Injection

#### secrets.json
```json
"OpenAIServiceOptions": {
    "ApiKey": "Your api key goes here",
    "Organization": "Your Organization Id goes here (optional)",
    "UseBeta": "true/false (optional)"
}
```
*(To use [user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows): Right-click your project name in "Solution Explorer", then click "Manage User Secrets". This is a good way to keep your API keys secure.)*

#### Program.cs
```csharp
serviceCollection.AddOpenAIService();
```

**OR**

```csharp
serviceCollection.AddOpenAIService(settings => { settings.ApiKey = Environment.GetEnvironmentVariable("MY_OPEN_AI_API_KEY"); });
```

After injecting your service, you can retrieve it from the service provider:
```csharp
var openAiService = serviceProvider.GetRequiredService<IOpenAIService>();
```

You can set a default model (optional):
```csharp
openAiService.SetDefaultModelId(Models.Gpt_4o);
```

## Chat GPT Sample
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
    Model = Models.Gpt_4o,
});
if (completionResult.Successful)
{
    Console.WriteLine(completionResult.Choices.First().Message.Content);
}
```

---
## Notes
Due to time constraints, not all methods have been thoroughly tested or fully documented. If you encounter any issues, please report them or submit a pull request. Your contributions are always appreciated.

Needless to say, I cannot accept responsibility for any damage caused by using the library.

## Changelog
### 8.5.1
- Introduced `IsDelta` into BaseResponseModel, which can help to determine if incoming data is part of the delta.
- 
### 8.5.0
- Assistant Stream now returns the `BaseResponse` type, but they can be cast to the appropriate types(`RunStepResponse`,`RunResponse`,`MessageResponse`). The reason for this change is that we realized the stream API returns multiple different object types rather than returning a single object type.
- The Base Response now has a `StreamEvent` field, which can be used to determine the type of event while streaming.


### [More Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)
---

For any issues, contributions, or feedback, feel free to reach out or submit a pull request.

Betalgo X: [Betalgo (@Betalgo) / X (twitter.com)](https://twitter.com/Betalgo)  
Betalgo Linkedin:  [Betalgo | LinkedIn](https://www.linkedin.com/company/betalgo-up )  
Tolga X: [Tolga Kayhan (@kayhantolga) / X (twitter.com)](https://twitter.com/kayhantolga)  
Tolga Linkedin: [Tolga Kayhan | LinkedIn](https://www.linkedin.com/in/kayhantolga/)  
