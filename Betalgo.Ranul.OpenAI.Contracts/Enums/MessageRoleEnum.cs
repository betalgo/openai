using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct AssistantMessageRoleEnum(string value) : IEquatable<AssistantMessageRoleEnum>
{
    public static AssistantMessageRoleEnum User { get; } = new("user");
    public static AssistantMessageRoleEnum Assistant { get; } = new("assistant");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(AssistantMessageRoleEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is AssistantMessageRoleEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(AssistantMessageRoleEnum left, AssistantMessageRoleEnum right) => left.Equals(right);
    public static bool operator !=(AssistantMessageRoleEnum left, AssistantMessageRoleEnum right) => !(left == right);

    public static implicit operator string(AssistantMessageRoleEnum format) => format.Value;
    public static implicit operator AssistantMessageRoleEnum(string value) => new(value);
    public sealed class Converter : JsonConverter<AssistantMessageRoleEnum>
    {
        public override AssistantMessageRoleEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, AssistantMessageRoleEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}