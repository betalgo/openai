using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ResponseFormat(string value) : IEquatable<ResponseFormat>
{
    public static ResponseFormat JsonSchema { get; } = new("json_schema");
    public static ResponseFormat JsonObject { get; } = new("json_object");
    public static ResponseFormat Text { get; } = new("text");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ResponseFormat other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ResponseFormat other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ResponseFormat left, ResponseFormat right) => left.Equals(right);
    public static bool operator !=(ResponseFormat left, ResponseFormat right) => !(left == right);

    public static implicit operator string(ResponseFormat format) => format.Value;
    public static implicit operator ResponseFormat(string value) => new(value);

    public sealed class Converter : JsonConverter<ResponseFormat>
    {
        public override ResponseFormat Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ResponseFormat value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}