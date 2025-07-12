using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct MessageContentTypeEnum(string value) : IEquatable<MessageContentTypeEnum>
{
    public static MessageContentTypeEnum ImageFile { get; } = new("image_file");
    public static MessageContentTypeEnum Text { get; } = new("text");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(MessageContentTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is MessageContentTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(MessageContentTypeEnum left, MessageContentTypeEnum right) => left.Equals(right);
    public static bool operator !=(MessageContentTypeEnum left, MessageContentTypeEnum right) => !(left == right);

    public static implicit operator string(MessageContentTypeEnum format) => format.Value;
    public static implicit operator MessageContentTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<MessageContentTypeEnum>
    {
        public override MessageContentTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, MessageContentTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
