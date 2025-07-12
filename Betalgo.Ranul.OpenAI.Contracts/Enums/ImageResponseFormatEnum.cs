using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageResponseFormatEnum(string value) : IEquatable<ImageResponseFormatEnum>
{
    public static ImageResponseFormatEnum Url { get; } = new("url");
    public static ImageResponseFormatEnum Base64 { get; } = new("b64_json");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageResponseFormatEnum other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageResponseFormatEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageResponseFormatEnum left, ImageResponseFormatEnum right) => left.Equals(right);
    public static bool operator !=(ImageResponseFormatEnum left, ImageResponseFormatEnum right) => !(left == right);

    public static implicit operator string(ImageResponseFormatEnum format) => format.Value;
    public static implicit operator ImageResponseFormatEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageResponseFormatEnum>
    {
        public override ImageResponseFormatEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageResponseFormatEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}