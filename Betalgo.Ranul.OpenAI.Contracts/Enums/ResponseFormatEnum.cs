using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ResponseFormatEnum(string value) : IEquatable<ResponseFormatEnum>
{
    public static ResponseFormatEnum JsonSchema { get; } = new("json_schema");
    public static ResponseFormatEnum JsonObject { get; } = new("json_object");
    public static ResponseFormatEnum Text { get; } = new("text");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ResponseFormatEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ResponseFormatEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ResponseFormatEnum left, ResponseFormatEnum right) => left.Equals(right);
    public static bool operator !=(ResponseFormatEnum left, ResponseFormatEnum right) => !(left == right);

    public static implicit operator string(ResponseFormatEnum format) => format.Value;
    public static implicit operator ResponseFormatEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ResponseFormatEnum>
    {
        public override ResponseFormatEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ResponseFormatEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}