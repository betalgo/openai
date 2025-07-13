using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ToolCallType(string value) : IEquatable<ToolCallType>
{
    public static ToolCallType CodeInterpreter { get; } = new("code_interpreter");
    public static ToolCallType FileSearch { get; } = new("file_search");
    public static ToolCallType Function { get; } = new("function");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ToolCallType other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ToolCallType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ToolCallType left, ToolCallType right) => left.Equals(right);
    public static bool operator !=(ToolCallType left, ToolCallType right) => !(left == right);

    public static implicit operator string(ToolCallType format) => format.Value;
    public static implicit operator ToolCallType(string value) => new(value);

    public sealed class Converter : JsonConverter<ToolCallType>
    {
        public override ToolCallType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ToolCallType value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
