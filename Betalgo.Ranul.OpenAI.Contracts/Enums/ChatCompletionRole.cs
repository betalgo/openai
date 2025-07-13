using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     The role of the author of a message
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ChatCompletionRole : IEquatable<ChatCompletionRole>
{
    /// <summary>
    ///     Underlying string value of the ChatCompletionRoleEnum.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ChatCompletionRole" /> struct.
    /// </summary>
    /// <param name="value">Underlying string value.</param>
    public ChatCompletionRole(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }


    /// <summary>
    ///     Literal <c>developer</c>.
    /// </summary>
    public static ChatCompletionRole Developer { get; } = new("developer");

    /// <summary>
    ///     Literal <c>system</c>.
    /// </summary>
    public static ChatCompletionRole System { get; } = new("system");

    /// <summary>
    ///     Literal <c>user</c>.
    /// </summary>
    public static ChatCompletionRole User { get; } = new("user");

    /// <summary>
    ///     Literal <c>assistant</c>.
    /// </summary>
    public static ChatCompletionRole Assistant { get; } = new("assistant");

    /// <summary>
    ///     Literal <c>tool</c>.
    /// </summary>
    public static ChatCompletionRole Tool { get; } = new("tool");

    /// <summary>
    ///     Literal <c>function</c>.
    /// </summary>
    [Obsolete("Deprecated")]
    public static ChatCompletionRole Function { get; } = new("function");

    public override string ToString() => Value;

    public bool Equals(ChatCompletionRole other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) =>
        obj is ChatCompletionRole other && Equals(other);

    public override int GetHashCode() =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    public static bool operator ==(ChatCompletionRole left, ChatCompletionRole right) => left.Equals(right);
    public static bool operator !=(ChatCompletionRole left, ChatCompletionRole right) => !(left == right);
    public static implicit operator string(ChatCompletionRole format) => format.Value;
    public static implicit operator ChatCompletionRole(string value) => new(value);

    /// <summary>System-Text-Json converter for <see cref="ChatCompletionRole" />.</summary>
    public sealed class Converter : JsonConverter<ChatCompletionRole>
    {
        public override ChatCompletionRole Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ChatCompletionRole value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}