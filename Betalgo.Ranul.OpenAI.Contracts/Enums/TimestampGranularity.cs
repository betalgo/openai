using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct TimestampGranularity(string value) : IEquatable<TimestampGranularity>
{
    public static TimestampGranularity Word { get; } = new("word");
    public static TimestampGranularity Segment { get; } = new("segment");

     public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(TimestampGranularity other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is TimestampGranularity other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(TimestampGranularity left, TimestampGranularity right) => left.Equals(right);
    public static bool operator !=(TimestampGranularity left, TimestampGranularity right) => !(left == right);
    public static implicit operator string(TimestampGranularity format) => format.Value;
    public static implicit operator TimestampGranularity(string value) => new(value);

    public sealed class Converter : JsonConverter<TimestampGranularity>
    {
        public override TimestampGranularity Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, TimestampGranularity value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}