using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageStyleEnum(string value) : IEquatable<ImageStyleEnum>
{
    public static ImageStyleEnum Vivid { get; } = new("vivid");
    public static ImageStyleEnum Natural { get; } = new("natural");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageStyleEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ImageStyleEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageStyleEnum left, ImageStyleEnum right) => left.Equals(right);
    public static bool operator !=(ImageStyleEnum left, ImageStyleEnum right) => !(left == right);

    public static implicit operator string(ImageStyleEnum format) => format.Value;
    public static implicit operator ImageStyleEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageStyleEnum>
    {
        public override ImageStyleEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageStyleEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
