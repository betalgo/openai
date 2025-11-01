using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/audio/createSpeech#audio-createspeech-response_format">
///         OpenAI API: response_format documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct CreateSpeechResponseFormat(string value) : IEquatable<CreateSpeechResponseFormat>
{
    public static CreateSpeechResponseFormat Mp3 { get; } = new("mp3");
    public static CreateSpeechResponseFormat Opus { get; } = new("opus");
    public static CreateSpeechResponseFormat Aac { get; } = new("aac");
    public static CreateSpeechResponseFormat Flac { get; } = new("flac");

     public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(CreateSpeechResponseFormat other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is CreateSpeechResponseFormat other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(CreateSpeechResponseFormat left, CreateSpeechResponseFormat right) => left.Equals(right);
    public static bool operator !=(CreateSpeechResponseFormat left, CreateSpeechResponseFormat right) => !(left == right);
    public static implicit operator string(CreateSpeechResponseFormat format) => format.Value;
    public static implicit operator CreateSpeechResponseFormat(string value) => new(value);

    public sealed class Converter : JsonConverter<CreateSpeechResponseFormat>
    {
        public override CreateSpeechResponseFormat Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, CreateSpeechResponseFormat value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}