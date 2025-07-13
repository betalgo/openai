using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageFileType(string value) : IEquatable<ImageFileType>
{
    public static ImageFileType Jpeg { get; } = new("JPEG");
    public static ImageFileType Png { get; } = new("PNG");
    public static ImageFileType Webp { get; } = new("WEBP");
    public static ImageFileType Gif { get; } = new("GIF");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageFileType other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ImageFileType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageFileType left, ImageFileType right) => left.Equals(right);
    public static bool operator !=(ImageFileType left, ImageFileType right) => !(left == right);

    public static implicit operator string(ImageFileType format) => format.Value;
    public static implicit operator ImageFileType(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageFileType>
    {
        public override ImageFileType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageFileType value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}