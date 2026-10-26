using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The retention policy for the prompt cache.
///     <see href="https://platform.openai.com/docs/guides/prompt-caching#prompt-cache-retention">OpenAI documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/modelresponseproperties.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct PromptCacheRetention(string value) : IEquatable<PromptCacheRetention>
{
    /// <summary>
    ///     In-memory caching, with shorter retention.
    /// </summary>
    public static PromptCacheRetention InMemory { get; } = new("in_memory");

    /// <summary>
    ///     Extended prompt caching, which keeps cached prefixes active for longer, up to a maximum of 24 hours.
    /// </summary>
    public static PromptCacheRetention TwentyFourHours { get; } = new("24h");

    /// <summary>
    ///     The underlying string value of the retention policy.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(PromptCacheRetention other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is PromptCacheRetention other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="PromptCacheRetention" /> values are equal.
    /// </summary>
    public static bool operator ==(PromptCacheRetention left, PromptCacheRetention right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="PromptCacheRetention" /> values are not equal.
    /// </summary>
    public static bool operator !=(PromptCacheRetention left, PromptCacheRetention right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="PromptCacheRetention" /> to a string.
    /// </summary>
    public static implicit operator string(PromptCacheRetention retention) => retention.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="PromptCacheRetention" />.
    /// </summary>
    public static implicit operator PromptCacheRetention(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="PromptCacheRetention" />.
    /// </summary>
    public sealed class Converter : JsonConverter<PromptCacheRetention>
    {
        /// <inheritdoc />
        public override PromptCacheRetention Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, PromptCacheRetention value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

