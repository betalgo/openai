using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct RequiredActionType(string value) : IEquatable<RequiredActionType>
{
    public static RequiredActionType SubmitToolOutputs { get; } = new("submit_tool_outputs");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(RequiredActionType other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is RequiredActionType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(RequiredActionType left, RequiredActionType right) => left.Equals(right);
    public static bool operator !=(RequiredActionType left, RequiredActionType right) => !(left == right);

    public static implicit operator string(RequiredActionType format) => format.Value;
    public static implicit operator RequiredActionType(string value) => new(value);

    public sealed class Converter : JsonConverter<RequiredActionType>
    {
        public override RequiredActionType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, RequiredActionType value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}