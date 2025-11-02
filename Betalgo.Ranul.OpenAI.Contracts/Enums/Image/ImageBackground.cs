using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Image;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/images/create#images_create-background">
///         OpenAI API documentation
///     </see>
///     Allows to set transparency for the background of the generated image(s).
///     This parameter is only supported for <c>gpt-image-1</c>. Must be one of
///     <c>transparent</c>, <c>opaque</c> or <c>auto</c> (default value). When <c>auto</c> is used, the
///     model will automatically determine the best background for the image.
///     <para />
///     If <c>transparent</c>, the output format needs to support transparency, so it
///     should be set to either <c>png</c> (default value) or <c>webp</c>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageBackground(string value) : IEquatable<ImageBackground>
{
    /// <summary>
    ///     Literal <c>transparent</c>.
    /// </summary>
    public static ImageBackground Transparent { get; } = new("transparent");

    /// <summary>
    ///     Literal <c>opaque</c>.
    /// </summary>
    public static ImageBackground Opaque { get; } = new("opaque");

    /// <summary>
    ///     Literal <c>auto</c>.
    /// </summary>
    public static ImageBackground Auto { get; } = new("auto");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageBackground other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageBackground other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageBackground left, ImageBackground right) => left.Equals(right);
    public static bool operator !=(ImageBackground left, ImageBackground right) => !(left == right);
    public static implicit operator string(ImageBackground format) => format.Value;
    public static implicit operator ImageBackground(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageBackground>
    {
        public override ImageBackground Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageBackground value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}