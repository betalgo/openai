![Betalgo Ranul OpenAI Github readme banner](https://github.com/user-attachments/assets/3a76387f-e033-4ee2-a9c7-2ebf047c4d90)

## Overview
A .NET Library for accessing OpenAI's API, provided as a community library. This Library allows you to integrate OpenAI's capabilities into your .NET applications with ease.

⭐ We appreciate your star, it helps! ![GitHub Repo stars](https://img.shields.io/github/stars/betalgo/openai)  
 #### Community Links
- [![Discord](https://img.shields.io/discord/1250841506785529916?label=Discord)](https://discord.gg/gfgHsWnGxy) Please come and help us build the .NET AI community  
- [![Static Badge](https://img.shields.io/badge/Reddit-Betalgo%20Developers-orange)](https://www.reddit.com/r/BetalgoDevelopers)
- [![Static Badge](https://img.shields.io/badge/Github-Discussions-black)](https://github.com/betalgo/openai/discussions)
 
### Install Packages
#### Core Library
⚠️ We now have new PackageId and new Namespace. ⚠️  
⚠️ `Betalgo.OpenAI` is now `Betalgo.Ranul.OpenAI` ⚠️  

[![Betalgo.Ranul.OpenAI](https://img.shields.io/nuget/v/Betalgo.Ranul.OpenAI?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.Ranul.OpenAI/)
```shell
Install-Package Betalgo.Ranul.OpenAI
```

#### Experimental Utilities Library
[![Betalgo.OpenAI.Utilities](https://img.shields.io/nuget/v/Betalgo.OpenAI.Utilities?style=for-the-badge)](https://www.nuget.org/packages/Betalgo.OpenAI.Utilities/)
```shell
Install-Package Betalgo.OpenAI.Utilities
```

## Documentation and Links
- [Realtime API](https://github.com/betalgo/openai/wiki/realtime) ✨NEW
- [Wiki Page](https://github.com/betalgo/openai/wiki)
- [Feature Availability Table](https://github.com/betalgo/openai/wiki/Feature-Availability)
- [Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)
- [Migration Guide for Breaking Changes](https://github.com/betalgo/openai/wiki/Migration-Guides-for-breaking-changes)
- [Contracts Upgrade Guide (9.2.0)](https://github.com/betalgo/openai/wiki/Contracts-Project:-Introduction-and-Upgrade-Guide)
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
var openAIService = new OpenAIService(new OpenAIOptions()
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
### 9.2.0
- Introduced `Betalgo.Ranul.OpenAI.Contracts` project for centralized request/response models, enums/value types, and capability interfaces.
- Replaced legacy image request models with Contracts equivalents:
  - `CreateImageRequest`, `CreateImageEditRequest`, `CreateImageVariationRequest`
- Reorganized image enums under `Betalgo.Ranul.OpenAI.Contracts.Enums.Image.*` and added value types (`ImageOutputFormat`, `ImageResponseFormat`, `ImageSize`, `ImageModeration`).
- Replaced `VoiceEnum` with `Voice` (value type). Replaced `MessageRole` with `ChatCompletionRole` and/or `AssistantMessageRole`.
- Added new response base types: `ResponseBase`, `ResponseBaseHeaderValues`, improved header parsing and usage exposure.
- Updated `IImageService` to consume Contracts request models and return `ImageResponse` for image create. Edit/variation remain legacy responses for now.
- This is the first step of a gradual migration; changes are kept minimal and may evolve after testing.
- See the detailed guide: [Contracts Upgrade Guide (9.2.0)](https://github.com/betalgo/openai/wiki/Contracts-Project:-Introduction-and-Upgrade-Guide)

### 9.1.0

### Image Generation
- Added `UsageModel` to include detailed token usage tracking for image generation responses.

### JSON Schema Validation
- Enhanced `FunctionParameters` with `MultipleOf`, `Minimum`, `Maximum`, and `Pattern` support.
- Added `SingleOrArrayToListConverter` to handle single values or arrays.

### Code Improvements
- Refactored `MessageContent` with expression-bodied helpers and added binary file support.
- Converted filter type definitions in `ToolDefinition` from `static string` to `const string` for clarity and performance.
- Simplified filter creation methods.

### Documentation and Formatting
- Improved comments and formatting in `ToolDefinition`.
- Fixed BOM character issue in `FunctionParameters`.

### 9.0.4
- Updated `Microsoft.Extensions.AI` to version `9.5.0`  

### 9.0.3
- Updated `Microsoft.Extensions.AI` to version `9.4.0-preview.1.25207.5`  
- Added new models to the model list

### 9.0.2  
- Updated `Microsoft.Extensions.AI` to version `9.3.0-preview.1.25114.11`  
- Added reasoning effort parameters  
- Added `o1` and `o3-mini` models to the model list

### 9.0.1
- Message list now accept RunId
- Upgraded to Microsoft.Extensions.AI version 9.0.1, which resolves the "Method not found: '!!0" error when used alongside other SDKs with different versions.

### 9.0.0
- .NET 9 support added.
- ⚠️ Support for .NET 6 and .NET 7 has ended.
- Fixed utility library issues and synced with latest version.

### [More Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)
---

For any issues, contributions, or feedback, feel free to reach out or submit a pull request.

Betalgo X: [Betalgo (@Betalgo) / X (twitter.com)](https://twitter.com/Betalgo)  
Betalgo Linkedin:  [Betalgo | LinkedIn](https://www.linkedin.com/company/betalgo-up )  
Tolga X: [Tolga Kayhan (@kayhantolga) / X (twitter.com)](https://twitter.com/kayhantolga)  
Tolga Linkedin: [Tolga Kayhan | LinkedIn](https://www.linkedin.com/in/kayhantolga/)  
