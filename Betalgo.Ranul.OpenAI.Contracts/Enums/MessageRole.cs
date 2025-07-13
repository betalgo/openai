using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct AssistantMessageRole(string value) : IEquatable<AssistantMessageRole>
{
    public static AssistantMessageRole User { get; } = new("user");
    public static AssistantMessageRole Assistant { get; } = new("assistant");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(AssistantMessageRole other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is AssistantMessageRole other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(AssistantMessageRole left, AssistantMessageRole right) => left.Equals(right);
    public static bool operator !=(AssistantMessageRole left, AssistantMessageRole right) => !(left == right);

    public static implicit operator string(AssistantMessageRole format) => format.Value;
    public static implicit operator AssistantMessageRole(string value) => new(value);
    public sealed class Converter : JsonConverter<AssistantMessageRole>
    {
        public override AssistantMessageRole Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, AssistantMessageRole value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}