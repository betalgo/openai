using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     Specifies the processing type used for serving the request.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/servicetier.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ServiceTier(string value) : IEquatable<ServiceTier>
{
    /// <summary>
    ///     The request will be processed with the service tier configured in the Project settings.
    ///     Unless otherwise configured, the Project will use 'default'.
    /// </summary>
    public static ServiceTier Auto { get; } = new("auto");

    /// <summary>
    ///     The request will be processed with the standard pricing and performance for the selected model.
    /// </summary>
    public static ServiceTier Default { get; } = new("default");

    /// <summary>
    ///     The request will be processed with flex processing.
    ///     <see href="https://platform.openai.com/docs/guides/flex-processing">Learn more</see>.
    /// </summary>
    public static ServiceTier Flex { get; } = new("flex");

    /// <summary>
    ///     The request will be processed with scale tier.
    /// </summary>
    public static ServiceTier Scale { get; } = new("scale");

    /// <summary>
    ///     The request will be processed with priority processing.
    ///     <see href="https://openai.com/api-priority-processing/">Learn more</see>.
    /// </summary>
    public static ServiceTier Priority { get; } = new("priority");

    /// <summary>
    ///     The underlying string value of the service tier.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ServiceTier other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ServiceTier other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ServiceTier" /> values are equal.
    /// </summary>
    public static bool operator ==(ServiceTier left, ServiceTier right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ServiceTier" /> values are not equal.
    /// </summary>
    public static bool operator !=(ServiceTier left, ServiceTier right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ServiceTier" /> to a string.
    /// </summary>
    public static implicit operator string(ServiceTier tier) => tier.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ServiceTier" />.
    /// </summary>
    public static implicit operator ServiceTier(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ServiceTier" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ServiceTier>
    {
        /// <inheritdoc />
        public override ServiceTier Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ServiceTier value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

