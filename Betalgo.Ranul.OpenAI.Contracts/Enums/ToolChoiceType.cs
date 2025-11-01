using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ToolChoiceType(string value) : IEquatable<ToolChoiceType>
{
    public static ToolChoiceType Function { get; } = new("function");
    public static ToolChoiceType Auto { get; } = new("auto");
    public static ToolChoiceType None { get; } = new("none");
    public static ToolChoiceType Required { get; } = new("required");

     public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ToolChoiceType other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ToolChoiceType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ToolChoiceType left, ToolChoiceType right) => left.Equals(right);
    public static bool operator !=(ToolChoiceType left, ToolChoiceType right) => !(left == right);
    public static implicit operator string(ToolChoiceType format) => format.Value;
    public static implicit operator ToolChoiceType(string value) => new(value);

    public sealed class Converter : JsonConverter<ToolChoiceType>
    {
        public override ToolChoiceType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ToolChoiceType value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}