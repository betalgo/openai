using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     A summary of the reasoning performed by the model. This can be useful for debugging
///     and understanding the model's reasoning process.
///     <see href="https://platform.openai.com/docs/guides/reasoning">OpenAI Reasoning Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/reasoning.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ReasoningSummary(string value) : IEquatable<ReasoningSummary>
{
    /// <summary>
    ///     Automatically determine the summary level.
    /// </summary>
    public static ReasoningSummary Auto { get; } = new("auto");

    /// <summary>
    ///     Provide a concise summary of the reasoning. Only supported for <c>computer-use-preview</c> models.
    /// </summary>
    public static ReasoningSummary Concise { get; } = new("concise");

    /// <summary>
    ///     Provide a detailed summary of the reasoning.
    /// </summary>
    public static ReasoningSummary Detailed { get; } = new("detailed");

    /// <summary>
    ///     The underlying string value of the summary type.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ReasoningSummary other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ReasoningSummary other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ReasoningSummary" /> values are equal.
    /// </summary>
    public static bool operator ==(ReasoningSummary left, ReasoningSummary right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ReasoningSummary" /> values are not equal.
    /// </summary>
    public static bool operator !=(ReasoningSummary left, ReasoningSummary right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ReasoningSummary" /> to a string.
    /// </summary>
    public static implicit operator string(ReasoningSummary summary) => summary.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ReasoningSummary" />.
    /// </summary>
    public static implicit operator ReasoningSummary(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ReasoningSummary" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ReasoningSummary>
    {
        /// <inheritdoc />
        public override ReasoningSummary Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ReasoningSummary value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

