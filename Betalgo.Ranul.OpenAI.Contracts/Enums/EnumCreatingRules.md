# Enum Creation Rules

## 1. Enum Naming Convention

- Enum names must use the format:  
  **ClassNamePropertyNameEnum**
- This format combines the class name and property name, followed by "Enum", for clear context and discoverability.

## 2. Documentation Link

- At the top of every enum class, include a documentation link as a `<see href="...">` inside the XML summary comment.
- This ensures developers can easily find the official reference for the enum.

## 3. Enum Template

Use the following template for all enum classes:
```csharp
/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/audio/createSpeech#audio-createspeech-response_format">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))] 
public readonly struct ClassNamePropertyNameEnum(string value) : IEquatable<ClassNamePropertyNameEnum> 
{ 
    public static ClassNamePropertyNameEnum MyVal1 { get; } = new("myVal1"); 
    public static ClassNamePropertyNameEnum MyVal2 { get; } = new("myVal2");
    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ClassNamePropertyNameEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ClassNamePropertyNameEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ClassNamePropertyNameEnum left, ClassNamePropertyNameEnum right) => left.Equals(right);
    public static bool operator !=(ClassNamePropertyNameEnum left, ClassNamePropertyNameEnum right) => !(left == right);

    public sealed class Converter : JsonConverter<ClassNamePropertyNameEnum>
    {
        public override ClassNamePropertyNameEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ClassNamePropertyNameEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
```
---

**Summary:**  
- Use the `ClassNamePropertyNameEnum` naming format.
- Always include a documentation link in the summary.
- Follow the provided template for structure and serialization.