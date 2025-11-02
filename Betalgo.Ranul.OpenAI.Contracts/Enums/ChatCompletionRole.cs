using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     The role of the author of a message
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ChatCompletionRole(string value) : IEquatable<ChatCompletionRole>
{
    public static ChatCompletionRole Developer { get; } = new("developer");
    public static ChatCompletionRole System { get; } = new("system");
    public static ChatCompletionRole User { get; } = new("user");
    public static ChatCompletionRole Assistant { get; } = new("assistant");
    public static ChatCompletionRole Tool { get; } = new("tool");

    [Obsolete("Deprecated")]
    public static ChatCompletionRole Function { get; } = new("function");
     public string Value { get; } = value;
    public override string ToString() => Value;
    public bool Equals(ChatCompletionRole other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    public override bool Equals(object? obj) => obj is ChatCompletionRole other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ChatCompletionRole left, ChatCompletionRole right) => left.Equals(right);
    public static bool operator !=(ChatCompletionRole left, ChatCompletionRole right) => !(left == right);
    public static implicit operator string(ChatCompletionRole format) => format.Value;
    public static implicit operator ChatCompletionRole(string value) => new(value);

    public sealed class Converter : JsonConverter<ChatCompletionRole>
    {
        public override ChatCompletionRole Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);
        public override void Write(Utf8JsonWriter writer, ChatCompletionRole value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}