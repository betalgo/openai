using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The status of a response generation.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ResponsesStatus(string value) : IEquatable<ResponsesStatus>
{
    /// <summary>
    ///     The response has been completed successfully.
    /// </summary>
    public static ResponsesStatus Completed { get; } = new("completed");

    /// <summary>
    ///     The response generation failed.
    /// </summary>
    public static ResponsesStatus Failed { get; } = new("failed");

    /// <summary>
    ///     The response is currently being generated.
    /// </summary>
    public static ResponsesStatus InProgress { get; } = new("in_progress");

    /// <summary>
    ///     The response generation was cancelled.
    /// </summary>
    public static ResponsesStatus Cancelled { get; } = new("cancelled");

    /// <summary>
    ///     The response is queued for processing.
    /// </summary>
    public static ResponsesStatus Queued { get; } = new("queued");

    /// <summary>
    ///     The response was not completed due to reaching limits or other constraints.
    /// </summary>
    public static ResponsesStatus Incomplete { get; } = new("incomplete");

    /// <summary>
    ///     The underlying string value of the status.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ResponsesStatus other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ResponsesStatus other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ResponsesStatus" /> values are equal.
    /// </summary>
    public static bool operator ==(ResponsesStatus left, ResponsesStatus right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ResponsesStatus" /> values are not equal.
    /// </summary>
    public static bool operator !=(ResponsesStatus left, ResponsesStatus right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="ResponsesStatus" /> to a string.
    /// </summary>
    public static implicit operator string(ResponsesStatus status) => status.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="ResponsesStatus" />.
    /// </summary>
    public static implicit operator ResponsesStatus(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ResponsesStatus" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ResponsesStatus>
    {
        /// <inheritdoc />
        public override ResponsesStatus Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ResponsesStatus value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

