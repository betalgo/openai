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

### 8.10.1
- Fixed an issue with the `Store` parameter being included in requests by default, causing errors with Azure OpenAI models. The parameter is now optional and excluded from serialization unless explicitly set.

### 8.10.0
- Added support for `Microsoft.Extensions.AI` `IChatClient` and `IEmbeddingGenerator` (more information will be coming soon to the Wiki).
- Added missing `Temperature` and `TopP` parameters to `AssistantResponse`.
- Added missing `Store` parameter to `ChatCompletionCreateRequest`.

- Breaking Changes:
   - ⚠️ `CreatedAt` parameter renamed to `CreatedAtUnix` and converted to `long` instead of `int`. Added `CreatedAt` parameter as `DateTimeOffset` type, which will automatically convert Unix time to `DateTime`.

### 8.9.0
- Realtime API implementation is completed. As usual this is the first version and it may contain bugs. Please report any issues you encounter.
- [Realtime Sample](https://github.com/betalgo/openai/wiki/realtime)

### 8.8.0

- **Compatibility Enhancement**: You can now use this library alongside the official OpenAI library and/or Semantic Kernel within the same project. The name changes in this update support this feature.

- **Namespace and Package ID Update**: The namespace and PackageId have been changed from `Betalgo.OpenAI` to `Betalgo.Ranul.OpenAI`.

- **OpenAI Naming Consistency**: We've standardized the use of "OpenAI" throughout the library, replacing any instances of "OpenAi" or other variations.

- **Migration Instructions**: Intellisense should assist you in updating your code. If it doesn't, please make the following changes manually:
  - Switch to the new NuGet package: `Betalgo.Ranul.OpenAI` instead of `Betalgo.OpenAI`.
  - Update all namespaces from `OpenAI` to `Betalgo.Ranul.OpenAI`.
  - Replace all occurrences of "OpenAi", "Openai", or any other variations with "OpenAI".

- **Need Help?**: If you encounter any issues, feel free to reach out via our Discord channel, Reddit channel, or GitHub discussions. We're happy to assist.

- **Feedback Welcomed**: If you notice any mistakes or missing name changes, please create an issue to let us know.

- **Utilities Library Status**: Please note that the Utilities library might remain broken for a while. I will focus on fixing it after completing the real-time API implementation.

### [More Change Logs](https://github.com/betalgo/openai/wiki/Change-Logs)
---

For any issues, contributions, or feedback, feel free to reach out or submit a pull request.

Betalgo X: [Betalgo (@Betalgo) / X (twitter.com)](https://twitter.com/Betalgo)  
Betalgo Linkedin:  [Betalgo | LinkedIn](https://www.linkedin.com/company/betalgo-up )  
Tolga X: [Tolga Kayhan (@kayhantolga) / X (twitter.com)](https://twitter.com/kayhantolga)  
Tolga Linkedin: [Tolga Kayhan | LinkedIn](https://www.linkedin.com/in/kayhantolga/)  
