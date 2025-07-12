using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ToolTypeEnum(string value) : IEquatable<ToolTypeEnum>
{
    public static ToolTypeEnum Function { get; } = new("function");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ToolTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ToolTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ToolTypeEnum left, ToolTypeEnum right) => left.Equals(right);
    public static bool operator !=(ToolTypeEnum left, ToolTypeEnum right) => !(left == right);

    public static implicit operator string(ToolTypeEnum format) => format.Value;
    public static implicit operator ToolTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ToolTypeEnum>
    {
        public override ToolTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ToolTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
