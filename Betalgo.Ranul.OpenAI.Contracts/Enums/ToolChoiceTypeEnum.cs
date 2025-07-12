using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ToolChoiceTypeEnum(string value) : IEquatable<ToolChoiceTypeEnum>
{
    public static ToolChoiceTypeEnum Function { get; } = new("function");
    public static ToolChoiceTypeEnum Auto { get; } = new("auto");
    public static ToolChoiceTypeEnum None { get; } = new("none");
    public static ToolChoiceTypeEnum Required { get; } = new("required");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ToolChoiceTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ToolChoiceTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ToolChoiceTypeEnum left, ToolChoiceTypeEnum right) => left.Equals(right);
    public static bool operator !=(ToolChoiceTypeEnum left, ToolChoiceTypeEnum right) => !(left == right);

    public static implicit operator string(ToolChoiceTypeEnum format) => format.Value;
    public static implicit operator ToolChoiceTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ToolChoiceTypeEnum>
    {
        public override ToolChoiceTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ToolChoiceTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
