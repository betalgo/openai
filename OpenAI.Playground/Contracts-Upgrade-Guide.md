## Contracts Project: Introduction and Upgrade Guide

This document introduces the new `Betalgo.Ranul.OpenAI.Contracts` project, explains why we’re adopting it, outlines what changed, and shows how to upgrade your code.

### Overview
- **New project**: `Betalgo.Ranul.OpenAI.Contracts` centralizes request/response models, enums/value types, and capability interfaces.
- **Direction**: We’ll roll this pattern out gradually across the SDK as models and APIs grow in complexity.
- **Roadmap**: The next update targets GPT‑5 features and the Reasoning API.
- **Stability**: These changes will be tested for a while and may evolve; we’ve kept them minimal to reduce friction.

### Why this change
- Complex model structures and API variants demand stronger, decoupled contracts.
- Centralized contracts give us clearer versioning, reusability, and easier migrations.

### What changed (high level)
- Added `Betalgo.Ranul.OpenAI.Contracts` with:
  - Requests: `Requests.Image.CreateImageRequest`, `CreateImageEditRequest`, `CreateImageVariationRequest`
  - Responses: `Responses.Image.ImageResponse`, shared base: `Responses.Base.ResponseBase`, `ResponseBaseHeaderValues`, `ResponseError`, `RateLimitInfo`, `OpenAIInfo`
  - Enums/value types: `Enums.*` and `Enums.Image.*` (e.g., `ImageSize`, `ImageOutputFormat`, `ImageResponseFormat`, `ImageModeration`, `Voice`, roles)
  - Types: `Types.FormFile`, `Types.FormFileOrList`, `Types.StringOrList`
  - Interfaces: `Interfaces.IHasImageSize`, `IHasImageBackground`, `IHasImageOutputFormat`, `IHasImageQuality`, `IHasModelImage`
- Removed legacy image request models from `OpenAI.SDK/ObjectModels/RequestModels` and replaced with Contracts requests.
- Introduced V2 HTTP helpers wired to `ResponseBase`; legacy helpers retained for backward compatibility.

### Impacted files (selected)
- Added: `Betalgo.Ranul.OpenAI.Contracts/**`
- Removed: `OpenAI.SDK/ObjectModels/RequestModels/ImageCreateRequest.cs`, `ImageEditCreateRequest.cs`, `ImageVariationCreateRequest.cs`, `SharedImageRequestBaseModel.cs`
- Updated: `OpenAI.SDK/Interfaces/IImageService.cs`, `OpenAI.SDK/Managers/OpenAIImage.cs`, Playground image tests
- Added (compat): `OpenAI.SDK/Extensions/HttpclientExtensionsLegacy.cs`

### Breaking changes and replacements

| Old type/namespace | New type/namespace |
| --- | --- |
| `OpenAI.SDK.ObjectModels.RequestModels.ImageCreateRequest` | `Betalgo.Ranul.OpenAI.Contracts.Requests.Image.CreateImageRequest` |
| `OpenAI.SDK.ObjectModels.RequestModels.ImageEditCreateRequest` | `Betalgo.Ranul.OpenAI.Contracts.Requests.Image.CreateImageEditRequest` |
| `OpenAI.SDK.ObjectModels.RequestModels.ImageVariationCreateRequest` | `Betalgo.Ranul.OpenAI.Contracts.Requests.Image.CreateImageVariationRequest` |
| `OpenAI.SDK.ObjectModels.SharedModels.SharedImageRequestBaseModel` | Replaced by small capability interfaces under `Betalgo.Ranul.OpenAI.Contracts.Interfaces` |
| `Enums.VoiceEnum` | `Betalgo.Ranul.OpenAI.Contracts.Enums.Voice` (value type) |
| `Enums.MessageRole` | `Betalgo.Ranul.OpenAI.Contracts.Enums.ChatCompletionRole` and/or `AssistantMessageRole` |
| Top-level `Enums.Image*` | `Betalgo.Ranul.OpenAI.Contracts.Enums.Image.*` (moved/clarified) |
| `Responses.Base.ResponseHeaderValues` | `Betalgo.Ranul.OpenAI.Contracts.Responses.Base.ResponseBaseHeaderValues` |

`IImageService` now consumes Contracts models and returns Contracts responses where applicable:

```csharp
Task<ImageResponse> CreateImage(CreateImageRequest imageCreate, CancellationToken cancellationToken = default);
Task<ImageCreateResponse> CreateImageEdit(CreateImageEditRequest imageEditCreateRequest, CancellationToken cancellationToken = default);
Task<ImageCreateResponse> CreateImageVariation(CreateImageVariationRequest imageEditCreateRequest, CancellationToken cancellationToken = default);
```

Notes:
- `CreateImage` returns `Contracts.Responses.Image.ImageResponse`.
- `CreateImageEdit` and `CreateImageVariation` currently return legacy `ImageCreateResponse` for compatibility (may unify later).

### What you need to update

1) Update usings/namespaces
- Remove: `using OpenAI.SDK.ObjectModels.RequestModels;`
- Add:
  - `using Betalgo.Ranul.OpenAI.Contracts.Requests.Image;`
  - `using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;`
  - Optionally: `using Betalgo.Ranul.OpenAI.Contracts.Types;`
  - For roles/voices: `using Betalgo.Ranul.OpenAI.Contracts.Enums;`

2) Switch to Contracts request models
- Image create:

```csharp
var imageResult = await sdk.Image.CreateImage(new()
{
    Prompt = "Laser cat eyes",
    N = 1,
    Size = ImageSize.Size256
});
```

- Image edit (supports single or multiple images via `FormFileOrList`):

```csharp
var imageResult = await sdk.Image.CreateImageEdit(new()
{
    Image = new FormFile(originalFileName, originalFile), // or: new List<FormFile> { ... }
    Mask = new(maskFileName, maskFile),
    Prompt = "A sunlit indoor lounge area with a pool containing a cat",
    N = 4,
    Size = ImageSize.Size1024,
    ResponseFormat = ImageResponseFormat.Url,
    User = "TestUser"
});
```

- Image variation:

```csharp
var imageResult = await sdk.Image.CreateImageVariation(new()
{
    Image = new(originalFileName, originalFile),
    N = 2,
    Size = ImageSize.Size256,
    ResponseFormat = ImageResponseFormat.Url,
    User = "TestUser"
});
```

3) Use the new file/union types where needed

```csharp
public class FormFile(string name, Stream data)
{
    public string Name { get; set; } = name;
    public Stream Data { get; set; } = data;
}

// Single or list with implicit conversions
FormFileOrList single = new FormFile("cat.png", stream);
FormFileOrList many = new List<FormFile> { new("a.png", a), new("b.png", b) };
```

4) Update enums/value types
- Roles:

```csharp
var role = ChatCompletionRole.Assistant; // developer, system, user, assistant, tool
```

- Voices:

```csharp
var voice = Voice.Alloy; // echo, fable, nova, onyx, shimmer
```

- Image response/output formats and size:

```csharp
var format = ImageOutputFormat.Png;        // jpeg, webp
var responseFormat = ImageResponseFormat.Url; // or: ImageResponseFormat.Base64
var size = ImageSize.Size1024;             // 256, 512, 1024, 1792x1024, 1024x1792
```

5) Response handling and headers
- `CreateImage` returns `ImageResponse` implementing `IDefaultResult<string>` and `IDefaultResults<string>`:

```csharp
var first = imageResponse.Result;      // first URL or base64
var all = imageResponse.Results;       // all URLs/base64 strings
```

- Headers via `ResponseBaseHeaderValues`:

```csharp
var headers = imageResponse.HeaderValues;
var requestId = headers?.XRequestId;
```

### Notable additions
- `ImageModeration` value type (`low`, `auto`) for gpt-image-1 moderation.
- `StringOrList` and `FormFileOrList` unions with converters for flexible JSON shape.
- `ResponseBase` now exposes `HttpStatusCode`, `HeaderValues`, `Error`, and `IsDelta` helpers.

### Compatibility and minimal disruption
- Legacy HTTP helpers (`OpenAI.SDK/Extensions/HttpclientExtensionsLegacy.cs`) remain for existing flows.
- Image enums were reorganized but preserve literal values; mostly namespace changes plus clarified names.
- `CreateImageEdit`/`CreateImageVariation` still return legacy response types for now.

### Known limitations and next steps
- This is the first pass; Contracts usage will expand across the SDK.
- Upcoming: GPT‑5 features, Reasoning API alignment, and unifying image edit/variation responses on Contracts base types.
- Expect minor adjustments during testing; we’ll keep migrations straightforward.


