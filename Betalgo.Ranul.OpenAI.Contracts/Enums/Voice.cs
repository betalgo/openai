using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct Voice(string value) : IEquatable<Voice>
{
    public static Voice Alloy { get; } = new("alloy");
    public static Voice Echo { get; } = new("echo");
    public static Voice Fable { get; } = new("fable");
    public static Voice Nova { get; } = new("nova");
    public static Voice Onyx { get; } = new("onyx");
    public static Voice Shimmer { get; } = new("shimmer");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(Voice other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is Voice other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(Voice left, Voice right) => left.Equals(right);
    public static bool operator !=(Voice left, Voice right) => !(left == right);
    public static implicit operator string(Voice format) => format.Value;
    public static implicit operator Voice(string value) => new(value);

    public sealed class Converter : JsonConverter<Voice>
    {
        public override Voice Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, Voice value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}






