using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;
/// <summary>
///     <see href="">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct VoiceEnum(string value) : IEquatable<VoiceEnum>
{
    public static VoiceEnum Alloy { get; } = new("alloy");
    public static VoiceEnum Echo { get; } = new("echo");
    public static VoiceEnum Fable { get; } = new("fable");
    public static VoiceEnum Nova { get; } = new("nova");
    public static VoiceEnum Onyx { get; } = new("onyx");
    public static VoiceEnum Shimmer { get; } = new("shimmer");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(VoiceEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is VoiceEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(VoiceEnum left, VoiceEnum right) => left.Equals(right);
    public static bool operator !=(VoiceEnum left, VoiceEnum right) => !(left == right);

    public static implicit operator string(VoiceEnum format) => format.Value;
    public static implicit operator VoiceEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<VoiceEnum>
    {
        public override VoiceEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, VoiceEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
