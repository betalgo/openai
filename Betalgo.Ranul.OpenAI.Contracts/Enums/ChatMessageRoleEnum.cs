using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums;

/// <summary>
///     <see href="https://platform.openai.com/docs/api-reference/chat/create#chat-create-messages">
///         OpenAI API documentation
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ChatMessageRoleEnum(string value) : IEquatable<ChatMessageRoleEnum>
{
    /// <summary>
    /// Developer-provided instructions that the model should follow, regardless of messages sent by the user. With o1 models and newer, use `developer` messages for this purpose instead.
    /// </summary>
    public static ChatMessageRoleEnum System { get; } = new("system");
    /// <summary>
    /// Developer-provided instructions that the model should follow, regardless of messages sent by the user. With o1 models and newer, developer messages replace the previous system
    /// </summary>
    public static ChatMessageRoleEnum Developer { get; } = new("developer");
    public static ChatMessageRoleEnum User { get; } = new("user");
    public static ChatMessageRoleEnum Assistant { get; } = new("assistant");
    public static ChatMessageRoleEnum Tool { get; } = new("tool");
    [Obsolete("Deprecated")]
    public static ChatMessageRoleEnum Function { get; } = new("function");

    public string Value { get; } = value;
    public override string ToString() => Value;

    public bool Equals(ChatMessageRoleEnum other) =>
        string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => obj is ChatMessageRoleEnum other && Equals(other);
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    public static bool operator ==(ChatMessageRoleEnum left, ChatMessageRoleEnum right) => left.Equals(right);
    public static bool operator !=(ChatMessageRoleEnum left, ChatMessageRoleEnum right) => !(left == right);

    public static implicit operator string(ChatMessageRoleEnum format) => format.Value;
    public static implicit operator ChatMessageRoleEnum(string value) => new(value);
    public sealed class Converter : JsonConverter<ChatMessageRoleEnum>
    {
        public override ChatMessageRoleEnum Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) =>
            new(reader.GetString()!);

        public override void Write(Utf8JsonWriter writer, ChatMessageRoleEnum value, JsonSerializerOptions o) =>
            writer.WriteStringValue(value.Value);
    }
}
