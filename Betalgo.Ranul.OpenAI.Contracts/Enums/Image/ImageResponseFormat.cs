using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Image;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageResponseFormat(string value) : IEquatable<ImageResponseFormat>
{
    public static ImageResponseFormat Url { get; } = new("url");
    public static ImageResponseFormat Base64 { get; } = new("b64_json");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageResponseFormat other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageResponseFormat other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageResponseFormat left, ImageResponseFormat right) => left.Equals(right);
    public static bool operator !=(ImageResponseFormat left, ImageResponseFormat right) => !(left == right);
    public static implicit operator string(ImageResponseFormat format) => format.Value;
    public static implicit operator ImageResponseFormat(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageResponseFormat>
    {
        public override ImageResponseFormat Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageResponseFormat value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}