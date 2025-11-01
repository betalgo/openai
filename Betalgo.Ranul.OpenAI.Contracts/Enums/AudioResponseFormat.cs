using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     The format of the output, in one of these options: <c>json</c>, <c>text</c>, <c>srt</c>, <c>verbose_json</c>, or
///     <c>vtt</c>. For <c>gpt-4o-transcribe</c> and <c>gpt-4o-mini-transcribe</c>, the only supported format is
///     <c>json</c>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct AudioResponseFormat(string value) : IEquatable<AudioResponseFormat>
{
    public static AudioResponseFormat Json { get; } = new("json");
    public static AudioResponseFormat Text { get; } = new("text");
    public static AudioResponseFormat Srt { get; } = new("srt");
    public static AudioResponseFormat VerboseJson { get; } = new("verbose_json");
    public static AudioResponseFormat Vtt { get; } = new("vtt");

    public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(AudioResponseFormat other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is AudioResponseFormat other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(AudioResponseFormat left, AudioResponseFormat right) => left.Equals(right);
    public static bool operator !=(AudioResponseFormat left, AudioResponseFormat right) => !(left == right);
    public static implicit operator string(AudioResponseFormat format) => format.Value;
    public static implicit operator AudioResponseFormat(string value) => new(value);

    public sealed class Converter : JsonConverter<AudioResponseFormat>
    {
        public override AudioResponseFormat Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, AudioResponseFormat value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}