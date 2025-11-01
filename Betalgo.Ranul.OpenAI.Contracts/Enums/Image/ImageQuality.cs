using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Image;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/images/create#images_create-quality">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageQuality(string value) : IEquatable<ImageQuality>
{
    /// <summary>
    ///     Auto (default value) will automatically select the best quality for the given model.
    /// </summary>
    public static ImageQuality Auto { get; } = new("auto");

    /// <summary>
    ///     High quality option for gpt-image-1.
    /// </summary>
    public static ImageQuality High { get; } = new("high");

    /// <summary>
    ///     Medium quality option for gpt-image-1.
    /// </summary>
    public static ImageQuality Medium { get; } = new("medium");

    /// <summary>
    ///     Low quality option for gpt-image-1.
    /// </summary>
    public static ImageQuality Low { get; } = new("low");

    /// <summary>
    ///     High definition quality option for dall-e-3.
    /// </summary>
    public static ImageQuality Hd { get; } = new("hd");

    /// <summary>
    ///     Standard quality option for dall-e-3 and the only option for dall-e-2.
    /// </summary>
    public static ImageQuality Standard { get; } = new("standard");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageQuality other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageQuality other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageQuality left, ImageQuality right) => left.Equals(right);
    public static bool operator !=(ImageQuality left, ImageQuality right) => !(left == right);
    public static implicit operator string(ImageQuality format) => format.Value;
    public static implicit operator ImageQuality(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageQuality>
    {
        public override ImageQuality Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageQuality value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}