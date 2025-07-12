using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/audio/createSpeech#audio-createspeech-response_format">
///         OpenAI API: response_format documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct CreateSpeechResponseFormatEnum(string value) : IEquatable<CreateSpeechResponseFormatEnum>
{
    public static CreateSpeechResponseFormatEnum Mp3 { get; } = new("mp3");
    public static CreateSpeechResponseFormatEnum Opus { get; } = new("opus");
    public static CreateSpeechResponseFormatEnum Aac { get; } = new("aac");
    public static CreateSpeechResponseFormatEnum Flac { get; } = new("flac");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(CreateSpeechResponseFormatEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is CreateSpeechResponseFormatEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(CreateSpeechResponseFormatEnum left, CreateSpeechResponseFormatEnum right) => left.Equals(right);
    public static bool operator !=(CreateSpeechResponseFormatEnum left, CreateSpeechResponseFormatEnum right) => !(left == right);

    public static implicit operator string(CreateSpeechResponseFormatEnum format) => format.Value;
    public static implicit operator CreateSpeechResponseFormatEnum(string value) => new(value);

    public sealed class Converter : JsonConverter<CreateSpeechResponseFormatEnum>
    {
        public override CreateSpeechResponseFormatEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, CreateSpeechResponseFormatEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}