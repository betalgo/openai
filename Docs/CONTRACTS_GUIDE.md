# Betalgo.Ranul.OpenAI.Contracts Creation Guide

This guide outlines the rules and standards for creating or updating contracts in the `Betalgo.Ranul.OpenAI.Contracts` project. We maintain strict consistency to ensure the library is robust, easy to maintain, and future-proof.

## 1. Source of Truth

We use the OpenApi specifications located in `Docs/openapi-split` as our source of truth.
*   **Schemas**: Located in `Docs/openapi-split/components/schemas/`
*   **Paths**: Located in `Docs/openapi-split/paths/`

**Rule**: Every C# contract must correspond to a schema definition in the YAML files. Do not invent properties or types that do not exist in the spec.

## 2. Naming Conventions

*   **Class Names**: Must match the YAML Schema name exactly (PascalCase).
*   **Property Names**:
    *   **C#**: PascalCase.
    *   **JSON**: strict `snake_case` matching the YAML property.
*   **Files**: One class per file. File name matches Class name.

## 3. Folder Structure

Organize files by domain logic, matching the general structure of the API (e.g., `Image`, `Chat`, `Assistant`).

```
Betalgo.Ranul.OpenAI.Contracts/
├── Requests/
│   └── Image/
│       └── CreateImageRequest.cs
├── Responses/
│   └── Image/
│       └── ImageResponse.cs
└── Enums/
    └── Image/
        └── ImageQuality.cs
```

## 4. Class Structure Rules

### 4.1. Base Interfaces & Classes
*   **Requests**: Must implement `IRequest`.
*   **Responses**: Must inherit `ResponseBase` and implement `IDefaultResult<T>`.

### 4.2. Constructors (MANDATORY)
Every contract class must have **at least two constructors**:
1.  **Empty Constructor**: Required for serialization.
2.  **Required Parameters Constructor**: Accepts all properties marked as `required` in the YAML.

**Example**:
```csharp
public class CreateImageRequest : IRequest
{
    // 1. Empty Constructor
    public CreateImageRequest() { }

    // 2. Required Parameters Constructor
    public CreateImageRequest(string prompt)
    {
        Prompt = prompt;
    }

    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = null!;
}
```

### 4.3. Shared Properties (Interfaces)
If a property (like `model`, `size`, `quality`) appears across multiple contracts, define or use an `IHas{PropertyName}` interface.

**Example**:
```csharp
public interface IHasImageSize
{
    [JsonPropertyName("size")]
    public ImageSize? Size { get; set; }
}
```

## 5. Handling Types

### 5.1. Primitive Types
*   `integer` -> `int?` (Always nullable unless strictly required and validated, but generally prefer nullable for optional fields).
*   `boolean` -> `bool?`
*   `number` -> `double?`

### 5.2. Enums (Smart Enums)
**CRITICAL**: Do NOT use standard C# `enum`. Use the `readonly struct` "Smart Enum" pattern.
*   **Reason**: The API may add new string values in the future. Standard enums would crash deserialization on unknown values. Smart structs preserve the string value.

**Template**:
```csharp
[JsonConverter(typeof(Converter))]
public readonly struct MyEnum(string value) : IEquatable<MyEnum>
{
    public static MyEnum Value1 { get; } = new("value1");
    public static MyEnum Value2 { get; } = new("value2");

    public string Value { get; } = value;
    
    // ... Boilerplate Equality & Operators ...
    public override string ToString() => Value;
    public bool Equals(MyEnum other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is MyEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(MyEnum left, MyEnum right) => left.Equals(right);
    public static bool operator !=(MyEnum left, MyEnum right) => !(left == right);
    public static implicit operator string(MyEnum format) => format.Value;
    public static implicit operator MyEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<MyEnum>
    {
        public override MyEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, MyEnum value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}
```

### 5.3. Unions (`anyOf` / `oneOf`)

#### Scenario A: String vs Enum (String Union)
If a field accepts a specific set of strings OR any string (e.g., Models), simply use the **Smart Enum** pattern. It handles both predefined static values and arbitrary strings.

#### Scenario B: Different Types (e.g., String vs List)
If a field accepts different JSON types (e.g., "prompt" can be a `string` or `array` of strings), you MUST create a custom Type with a custom `JsonConverter`.

**Do NOT** use `object` or `dynamic`.

**Example (`StringOrList`)**:
```csharp
[JsonConverter(typeof(StringOrListConverter))]
public class StringOrList
{
    public StringOrList(string? value) { AsString = value; }
    public StringOrList(List<string>? value) { AsList = value; }

    public string? AsString { get; }
    public List<string>? AsList { get; }
    
    // Implicit operators for ease of use
    public static implicit operator StringOrList(string value) => new(value);
}
```

## 6. Documentation

*   Copy the `description` from YAML to the C# XML `/// <summary>`.
*   Convert Markdown links `[Link](url)` to `<see href="url">Link</see>`.
*   Convert backticks `` `code` `` to `<c>code</c>`.
*   Add a class-level `<see href="...">` link to the official OpenAI API reference if available.
*   Add a class-level `<see href="...">` link to the source definition in the GitHub repository.

**Example**:
```csharp
/// <summary>
///     The format in which the generated images are returned. Must be one of <c>url</c> or <c>b64_json</c>.
///     <see href="https://platform.openai.com/docs/api-reference/images">Learn more</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/createimagerequest.yml">
///         Source Definition
///     </see>
/// </summary>
```

