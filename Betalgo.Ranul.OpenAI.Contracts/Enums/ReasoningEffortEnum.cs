using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ReasoningEffortEnum(string value) : IEquatable<ReasoningEffortEnum>
{
    public static ReasoningEffortEnum Low { get; } = new("low");
    public static ReasoningEffortEnum Medium { get; } = new("medium");
    public static ReasoningEffortEnum High { get; } = new("high");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ReasoningEffortEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ReasoningEffortEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ReasoningEffortEnum left, ReasoningEffortEnum right) => left.Equals(right);
    public static bool operator !=(ReasoningEffortEnum left, ReasoningEffortEnum right) => !(left == right);

    public static implicit operator string(ReasoningEffortEnum format) => format.Value;
    public static implicit operator ReasoningEffortEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ReasoningEffortEnum>
    {
        public override ReasoningEffortEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ReasoningEffortEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
