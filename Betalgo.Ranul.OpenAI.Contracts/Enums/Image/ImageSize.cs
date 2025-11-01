using System.Text.Json;
using System.Text.Json.Serialization;

// ReSharper disable InconsistentNaming

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Image;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageSize(string value) : IEquatable<ImageSize>

{
    public static ImageSize Size256 { get; } = new("256x256");
    public static ImageSize Size512 { get; } = new("512x512");
    public static ImageSize Size1024 { get; } = new("1024x1024");

    /// <summary>
    ///     Only dall-e-3 model
    /// </summary>

    public static ImageSize Size1792x1024 { get; } = new("1792x1024");

    /// <summary>
    ///     Only dall-e-3 model
    /// </summary>
    public static ImageSize Size1024x1792 { get; } = new("1024x1792");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageSize other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageSize other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageSize left, ImageSize right) => left.Equals(right);
    public static bool operator !=(ImageSize left, ImageSize right) => !(left == right);
    public static implicit operator string(ImageSize format) => format.Value;
    public static implicit operator ImageSize(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageSize>
    {
        public override ImageSize Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageSize value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}