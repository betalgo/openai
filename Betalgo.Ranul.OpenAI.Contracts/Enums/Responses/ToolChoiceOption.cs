using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     Controls which (if any) tool is called by the model.
///     <see href="https://platform.openai.com/docs/api-reference/responses/create">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/toolchoiceoptions.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ToolChoiceOption(string value) : IEquatable<ToolChoiceOption>
{
    /// <summary>
    ///     The model will not call any tool and instead generates a message.
    /// </summary>
    public static ToolChoiceOption None { get; } = new("none");

    /// <summary>
    ///     The model can pick between generating a message or calling one or more tools.
    /// </summary>
    public static ToolChoiceOption Auto { get; } = new("auto");

    /// <summary>
    ///     The model must call one or more tools.
    /// </summary>
    public static ToolChoiceOption Required { get; } = new("required");

    /// <summary>
    ///     The underlying string value.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ToolChoiceOption other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ToolChoiceOption other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ToolChoiceOption" /> values are equal.
    /// </summary>
    public static bool operator ==(ToolChoiceOption left, ToolChoiceOption right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ToolChoiceOption" /> values are not equal.
    /// </summary>
    public static bool operator !=(ToolChoiceOption left, ToolChoiceOption right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ToolChoiceOption" /> to a string.
    /// </summary>
    public static implicit operator string(ToolChoiceOption option) => option.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ToolChoiceOption" />.
    /// </summary>
    public static implicit operator ToolChoiceOption(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ToolChoiceOption" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ToolChoiceOption>
    {
        /// <inheritdoc />
        public override ToolChoiceOption Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ToolChoiceOption value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

