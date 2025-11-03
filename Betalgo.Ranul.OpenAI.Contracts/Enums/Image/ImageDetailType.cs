using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Image;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageDetailType(string value) : IEquatable<ImageDetailType>
{
    public static ImageDetailType High { get; } = new("high");
    public static ImageDetailType Low { get; } = new("low");
    public static ImageDetailType Auto { get; } = new("auto");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ImageDetailType other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ImageDetailType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageDetailType left, ImageDetailType right) => left.Equals(right);
    public static bool operator !=(ImageDetailType left, ImageDetailType right) => !(left == right);
    public static implicit operator string(ImageDetailType format) => format.Value;
    public static implicit operator ImageDetailType(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageDetailType>
    {
        public override ImageDetailType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ImageDetailType value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}