using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct MessageContentType(string value) : IEquatable<MessageContentType>
{
    public static MessageContentType ImageFile { get; } = new("image_file");
    public static MessageContentType Text { get; } = new("text");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(MessageContentType other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is MessageContentType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(MessageContentType left, MessageContentType right) => left.Equals(right);
    public static bool operator !=(MessageContentType left, MessageContentType right) => !(left == right);

    public static implicit operator string(MessageContentType format) => format.Value;
    public static implicit operator MessageContentType(string value) => new(value);

    public sealed class Converter : JsonConverter<MessageContentType>
    {
        public override MessageContentType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, MessageContentType value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
