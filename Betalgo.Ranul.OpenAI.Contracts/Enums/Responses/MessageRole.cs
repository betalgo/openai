using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The role of a message in the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct MessageRole(string value) : IEquatable<MessageRole>
{
    /// <summary>
    ///     User role - messages from the user.
    /// </summary>
    public static MessageRole User { get; } = new("user");

    /// <summary>
    ///     Assistant role - messages from the AI assistant.
    /// </summary>
    public static MessageRole Assistant { get; } = new("assistant");

    /// <summary>
    ///     System role - system-level instructions.
    /// </summary>
    public static MessageRole System { get; } = new("system");

    /// <summary>
    ///     Developer role - developer-level instructions (takes precedence over user).
    /// </summary>
    public static MessageRole Developer { get; } = new("developer");

    /// <summary>
    ///     The underlying string value of the role.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(MessageRole other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MessageRole other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="MessageRole" /> values are equal.
    /// </summary>
    public static bool operator ==(MessageRole left, MessageRole right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="MessageRole" /> values are not equal.
    /// </summary>
    public static bool operator !=(MessageRole left, MessageRole right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="MessageRole" /> to a string.
    /// </summary>
    public static implicit operator string(MessageRole role) => role.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="MessageRole" />.
    /// </summary>
    public static implicit operator MessageRole(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="MessageRole" />.
    /// </summary>
    public sealed class Converter : JsonConverter<MessageRole>
    {
        /// <inheritdoc />
        public override MessageRole Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MessageRole value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

