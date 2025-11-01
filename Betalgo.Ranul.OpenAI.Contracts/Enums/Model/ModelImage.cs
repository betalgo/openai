using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Model;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ModelImage(string value) : IEquatable<ModelImage>
{
    public static ModelImage DallE2 { get; } = new("dall-e-2");
    public static ModelImage DallE3 { get; } = new("dall-e-3");
    public static ModelImage GptImage1 { get; } = new("gpt-image-1");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ModelImage other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ModelImage other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ModelImage left, ModelImage right) => left.Equals(right);
    public static bool operator !=(ModelImage left, ModelImage right) => !(left == right);
    public static implicit operator string(ModelImage format) => format.Value;
    public static implicit operator ModelImage(string value) => new(value);

    public sealed class Converter : JsonConverter<ModelImage>
    {
        public override ModelImage Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ModelImage value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}