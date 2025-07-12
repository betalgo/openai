using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;


/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct AnnotationTypeEnum(string value) : IEquatable<AnnotationTypeEnum>
{
    public static AnnotationTypeEnum FileCitation { get; } = new("file_citation");
    public static AnnotationTypeEnum FilePath { get; } = new("file_path");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(AnnotationTypeEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is AnnotationTypeEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(AnnotationTypeEnum left, AnnotationTypeEnum right) => left.Equals(right);
    public static bool operator !=(AnnotationTypeEnum left, AnnotationTypeEnum right) => !(left == right);

    public static implicit operator string(AnnotationTypeEnum format) => format.Value;
    public static implicit operator AnnotationTypeEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<AnnotationTypeEnum>
    {
        public override AnnotationTypeEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, AnnotationTypeEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
