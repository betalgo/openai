# Migration Guide: OpenAI.SDK to Betalgo.Ranul.OpenAI.Contracts

This guide outlines the process for migrating Object Models from the legacy `OpenAI.SDK` project to the new `Betalgo.Ranul.OpenAI.Contracts` library. The goal is to establish a strict, schema-driven contract layer that acts as the single source of truth.

## 1. Pre-requisites

Before starting any migration task, you **MUST** read and understand the **[Contracts Creation Guide](../Docs/CONTRACTS_GUIDE.md)**.
That document defines the strict rules for:
-   Naming conventions
-   Folder structure
-   Smart Enums
-   Constructor requirements
-   Type handling

## 2. General Workflow

To migrate a set of models (e.g., "Chat Completion" or "Assistants"), follow these steps:

### Step 1: Choose a Domain
Select a specific domain to migrate. Avoid mixing domains in a single task.
*   *Example*: "Images", "Chat", "Assistants", "Embeddings".

### Step 2: Identify Legacy Models
Locate the existing models in `OpenAI.SDK/ObjectModels/`. They are usually scattered across:
*   `RequestModels/`
*   `ResponseModels/`
*   `SharedModels/`

### Step 3: Locate Source of Truth
Find the corresponding OpenAPI definition in `Docs/openapi-split`.
*   **Schemas**: `Docs/openapi-split/components/schemas/`
*   **Paths**: `Docs/openapi-split/paths/` (to see which schemas are used in requests/responses).

> **Crucial**: The new contract MUST be based on the **YAML Schema**, not the old C# class. The old class might have properties that don't exist in the official API or are named incorrectly.

### Step 4: Create the New Contract
Create the new file in `Betalgo.Ranul.OpenAI.Contracts/` following the folder structure defined in the [Contracts Guide](../Docs/CONTRACTS_GUIDE.md).

#### Checklist for New Contract:
1.  **Namespace**: `Betalgo.Ranul.OpenAI.Contracts.{Domain}.{SubDomain}` (if applicable).
2.  **Constructors**:
    *   Empty constructor.
    *   `required` parameters constructor.
3.  **Properties**:
    *   PascalCase for C#.
    *   `[JsonPropertyName("snake_case")]` matching YAML exactly.
    *   Use `Smart Enum` structs for enums.
    *   Use `IHas...` interfaces for shared properties (e.g., `IHasModel`).
4.  **Documentation**:
    *   `<summary>` from YAML description.
    *   Link to OpenAI API reference.
    *   Link to YAML source file.

### Step 5: Handle Divergences
If the legacy model has helper methods or properties that are *not* in the YAML schema:
1.  **Do not** add them to the data contract.
2.  These should be moved to Extension methods or separate Logic classes in the SDK, not the Contract library. Contracts must remain pure DTOs.

### Step 6: Review and Verify
*   Does the file name match the class name?
*   Are all `required` fields in the constructor?
*   Are standard `enum`s replaced with `readonly struct` Smart Enums?
*   Does it compile?

### Step 7: Update Managers and Services
Once contracts are created, update the SDK implementation to use them.
1.  Locate the service interface (e.g., `IAudioService.cs`) and implementation (e.g., `OpenAIAudioService.cs`).
2.  Replace legacy request/response types with the new Contracts.
3.  Update method signatures.
4.  Update implementation logic (e.g., mapping properties to `MultipartFormDataContent`).
5.  **Delete** the old legacy models from `OpenAI.SDK/ObjectModels/`.

### Step 8: Update TestHelpers
Tests often rely on mock services or helpers. You must update these to reflect the breaking changes.
1.  Check `OpenAI.Playground/TestHelpers` or `OpenAI.UtilitiesPlayground/TestHelpers`.
2.  Update any mock implementations or helper methods that instantiate the old models.

### Step 9: Changelog & Breaking Changes (CRITICAL)
At the end of the migration task, you must document what changed. This is vital for users updating from the old SDK to the new Contracts.

Create a domain-specific changelog file (e.g., `CHANGELOG_IMAGES.md`) in the root of `Betalgo.Ranul.OpenAI.Contracts`. Separate entries into:

1.  **Breaking Changes**:
    *   Renamed properties (Old Name -> New Name).
    *   Type changes (e.g., `string` -> `List<string>` or `Enum`).
    *   Removed properties (if they don't exist in the YAML spec).
2.  **Changes**:
    *   New properties added.
    *   Namespace changes (always expected).

**Example Changelog Entry**:
```markdown
## [Image Domain]
### Breaking Changes
- `ImageCreateRequest.N` -> `CreateImageRequest.N` (Class renamed)
- `ImageCreateRequest.ImageSize` (enum) -> `CreateImageRequest.Size` (Smart Enum)
- Removed `User` property (not in current spec).

### Changes
- Added `Quality` property.
- Moved to namespace `Betalgo.Ranul.OpenAI.Contracts.Requests.Image`.
```

## 3. Example Migration Walkthrough

**Task**: Migrate `OpenAI.SDK/ObjectModels/RequestModels/ImageCreateRequest.cs` (Hypothetical legacy name).

1.  **Source**: Found `create_image_request` in `Docs/openapi-split/components/schemas/createimagerequest.yml`.
2.  **Target**: Create `Betalgo.Ranul.OpenAI.Contracts/Requests/Image/CreateImageRequest.cs`.
3.  **Implementation**:
    ```csharp
    namespace Betalgo.Ranul.OpenAI.Contracts.Requests.Image;

    public class CreateImageRequest : IRequest
    {
        public CreateImageRequest() { }
        public CreateImageRequest(string prompt) { Prompt = prompt; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = null!;

        [JsonPropertyName("n")]
        public int? N { get; set; }
        
        // ... other properties matching YAML ...
    }
    ```
4.  **Update Service**:
    *   Open `IImageService.cs`.
    *   Change `Task<ImageCreateResponse> CreateImage(ImageCreateRequest request)` to `Task<ImageResponse> CreateImage(CreateImageRequest request)`.
    *   Update `OpenAIImageService.cs` implementation.
