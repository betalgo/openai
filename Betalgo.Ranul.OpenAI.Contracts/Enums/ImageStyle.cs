using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageStyle(string value) : IEquatable<ImageStyle>
{
    public static ImageStyle Vivid { get; } = new("vivid");
    public static ImageStyle Natural { get; } = new("natural");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageStyle other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ImageStyle other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageStyle left, ImageStyle right) => left.Equals(right);
    public static bool operator !=(ImageStyle left, ImageStyle right) => !(left == right);

    public static implicit operator string(ImageStyle format) => format.Value;
    public static implicit operator ImageStyle(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageStyle>
    {
        public override ImageStyle Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageStyle value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
