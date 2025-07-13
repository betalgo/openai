using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;


/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct AnnotationType(string value) : IEquatable<AnnotationType>
{
    public static AnnotationType FileCitation { get; } = new("file_citation");
    public static AnnotationType FilePath { get; } = new("file_path");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(AnnotationType other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is AnnotationType other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(AnnotationType left, AnnotationType right) => left.Equals(right);
    public static bool operator !=(AnnotationType left, AnnotationType right) => !(left == right);

    public static implicit operator string(AnnotationType format) => format.Value;
    public static implicit operator AnnotationType(string value) => new(value);

    public sealed class Converter : JsonConverter<AnnotationType>
    {
        public override AnnotationType Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, AnnotationType value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
