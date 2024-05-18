
# .NET SDK for OpenAI

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
[@betalgo](https://github.com/betalgo), [Laser Cat Eyes](https://lasercateyes.com/), [@tylerje](https://github.com/tylerje), [@oferavnery](https://github.com/oferavnery), [@MayDay-wpf](https://github.com/MayDay-wpf), [@AnukarOP](https://github.com/AnukarOP), [@Removable](https://github.com/Removable), [@Scar11](https://github.com/Scar11)

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
#### Library Renaming
This library was previously known as `Betalgo.OpenAI.GPT3`, and now it has a new package ID: `Betalgo.OpenAI`.

Due to time constraints, not all methods have been thoroughly tested or fully documented. If you encounter any issues, please report them or submit a pull request. Your contributions are always appreciated.

I initially developed this SDK for my personal use and decided to share it with the community. As I have not maintained open-source projects before, any assistance or feedback would be greatly appreciated. Feel free to reach out with your suggestions.

Please be aware that future releases may frequently include breaking changes. Consider this before deciding to use the library. I cannot accept responsibility for any damage caused by using the library. You are free to explore alternative libraries or the OpenAI Web-API if you feel this is not suitable for your purposes.


## Changelog
### 8.2.2
- Assistant (Beta) feature is now available in the main package. Be aware there might still be bugs due to the beta status of the feature and the SDK itself. Please report any issues you encounter.
- Use `"UseBeta": true` in your config file or `serviceCollection.AddOpenAIService(r => r.UseBeta = true);` or `new OpenAiOptions { UseBeta = true }` in your service registration to enable Assistant features.
- Expect more frequent breaking changes around the assistant API due to its beta nature.
- All Assistant endpoints are implemented except for streaming functionality, which will be added soon.
- The Playground has samples for every endpoint usage, but lacks a complete implementation for the Assistant APIs. Refer to [Assistants overview - OpenAI API](https://platform.openai.com/docs/assistants/overview) for more details.
- Special thanks to all contributors for making this version possible!

#### Other Changes:
- Fixed a bug with multiple tools calling in stream mode.
- Added error handling for streaming.
- Added usage information for streaming (use `StreamOptions = new(){IncludeUsage = true,}` to get usage information).
- Added **timestamp_granularities[]** for Create transcription to provide the timestamp of every word.

### [More Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)
---

For any issues, contributions, or feedback, feel free to reach out or submit a pull request.
