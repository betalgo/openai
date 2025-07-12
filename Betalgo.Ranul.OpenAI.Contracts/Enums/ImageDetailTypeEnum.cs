using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ImageDetailTypeEnum(string value) : IEquatable<ImageDetailTypeEnum>
{
    public static ImageDetailTypeEnum High { get; } = new("high");
    public static ImageDetailTypeEnum Low { get; } = new("low");
    public static ImageDetailTypeEnum Auto { get; } = new("auto");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ImageDetailTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ImageDetailTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ImageDetailTypeEnum left, ImageDetailTypeEnum right) => left.Equals(right);
    public static bool operator !=(ImageDetailTypeEnum left, ImageDetailTypeEnum right) => !(left == right);

    public static implicit operator string(ImageDetailTypeEnum format) => format.Value;
    public static implicit operator ImageDetailTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<ImageDetailTypeEnum>
    {
        public override ImageDetailTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ImageDetailTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
