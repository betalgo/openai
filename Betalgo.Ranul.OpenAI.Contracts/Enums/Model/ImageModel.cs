using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Model;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageModel(string value) : IEquatable<ImageModel>
{
    public static ImageModel DallE2 { get; } = new("dall-e-2");
    public static ImageModel DallE3 { get; } = new("dall-e-3");
    public static ImageModel GptImage1 { get; } = new("gpt-image-1");
    public static ImageModel GptImage1Mini { get; } = new("gpt-image-1-mini");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageModel other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageModel other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageModel left, ImageModel right) => left.Equals(right);
    public static bool operator !=(ImageModel left, ImageModel right) => !(left == right);
    public static implicit operator string(ImageModel format) => format.Value;
    public static implicit operator ImageModel(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageModel>
    {
        public override ImageModel Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageModel value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}