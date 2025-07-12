using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageFileTypeEnum(string value) : IEquatable<ImageFileTypeEnum>
{
    public static ImageFileTypeEnum Jpeg { get; } = new("JPEG");
    public static ImageFileTypeEnum Png { get; } = new("PNG");
    public static ImageFileTypeEnum Webp { get; } = new("WEBP");
    public static ImageFileTypeEnum Gif { get; } = new("GIF");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageFileTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ImageFileTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageFileTypeEnum left, ImageFileTypeEnum right) => left.Equals(right);
    public static bool operator !=(ImageFileTypeEnum left, ImageFileTypeEnum right) => !(left == right);

    public static implicit operator string(ImageFileTypeEnum format) => format.Value;
    public static implicit operator ImageFileTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageFileTypeEnum>
    {
        public override ImageFileTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageFileTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}