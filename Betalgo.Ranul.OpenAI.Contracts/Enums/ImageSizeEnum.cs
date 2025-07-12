using System.Text.Json;
using System.Text.Json.Serialization;
// ReSharper disable InconsistentNaming

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageSizeEnum(string value) : IEquatable<ImageSizeEnum>

{ 
    public static ImageSizeEnum Size256 { get; } = new("256x256");
    public static ImageSizeEnum Size512 { get; } = new("512x512");
    public static ImageSizeEnum Size1024 { get; } = new("1024x1024");

    /// <summary>
    ///     Only dall-e-3 model
    /// </summary>

    public static ImageSizeEnum Size1792x1024 { get; } = new("1792x1024");

    /// <summary>
    ///     Only dall-e-3 model
    /// </summary>
    public static ImageSizeEnum Size1024x1792 { get; } = new("1024x1792");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageSizeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ImageSizeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageSizeEnum left, ImageSizeEnum right) => left.Equals(right);
    public static bool operator !=(ImageSizeEnum left, ImageSizeEnum right) => !(left == right);

    public static implicit operator string(ImageSizeEnum format) => format.Value;
    public static implicit operator ImageSizeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageSizeEnum>
    {
        public override ImageSizeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageSizeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
