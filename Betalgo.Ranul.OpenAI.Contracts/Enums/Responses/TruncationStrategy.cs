using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The truncation strategy to use for the model response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/responseproperties.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct TruncationStrategy(string value) : IEquatable<TruncationStrategy>
{
    /// <summary>
    ///     If the input to this Response exceeds the model's context window size,
    ///     the model will truncate the response to fit the context window by dropping
    ///     items from the beginning of the conversation.
    /// </summary>
    public static TruncationStrategy Auto { get; } = new("auto");

    /// <summary>
    ///     If the input size will exceed the context window size for a model,
    ///     the request will fail with a 400 error. This is the default.
    /// </summary>
    public static TruncationStrategy Disabled { get; } = new("disabled");

    /// <summary>
    ///     The underlying string value of the truncation strategy.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(TruncationStrategy other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is TruncationStrategy other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="TruncationStrategy" /> values are equal.
    /// </summary>
    public static bool operator ==(TruncationStrategy left, TruncationStrategy right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="TruncationStrategy" /> values are not equal.
    /// </summary>
    public static bool operator !=(TruncationStrategy left, TruncationStrategy right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="TruncationStrategy" /> to a string.
    /// </summary>
    public static implicit operator string(TruncationStrategy strategy) => strategy.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="TruncationStrategy" />.
    /// </summary>
    public static implicit operator TruncationStrategy(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="TruncationStrategy" />.
    /// </summary>
    public sealed class Converter : JsonConverter<TruncationStrategy>
    {
        /// <inheritdoc />
        public override TruncationStrategy Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, TruncationStrategy value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

