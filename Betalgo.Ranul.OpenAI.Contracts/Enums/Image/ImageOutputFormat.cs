using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Image;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/images/create#images_create-output_format">
///         OpenAI API documentation
///     </see>
///     The format in which the generated images are returned. This parameter is
///     only supported for <c>gpt-image-1</c>. Must be one of <c>png</c>, <c>jpeg</c>, or <c>webp</c>.
///     The default value is <c>png</c>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageOutputFormat(string value) : IEquatable<ImageOutputFormat>
{
    /// <summary>
    ///     Literal <c>png</c>.
    /// </summary>
    public static ImageOutputFormat Png { get; } = new("png");

    /// <summary>
    ///     Literal <c>jpeg</c>.
    /// </summary>
    public static ImageOutputFormat Jpeg { get; } = new("jpeg");

    /// <summary>
    ///     Literal <c>webp</c>.
    /// </summary>
    public static ImageOutputFormat Webp { get; } = new("webp");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageOutputFormat other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageOutputFormat other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageOutputFormat left, ImageOutputFormat right) => left.Equals(right);
    public static bool operator !=(ImageOutputFormat left, ImageOutputFormat right) => !(left == right);
    public static implicit operator string(ImageOutputFormat format) => format.Value;
    public static implicit operator ImageOutputFormat(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageOutputFormat>
    {
        public override ImageOutputFormat Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageOutputFormat value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}