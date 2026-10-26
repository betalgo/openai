using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The status of an item in the Responses API.
///     <see href="https://platform.openai.com/docs/api-reference/responses">OpenAI API documentation</see>.
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct ItemStatus(string value) : IEquatable<ItemStatus>
{
    /// <summary>
    ///     The item is currently being processed.
    /// </summary>
    public static ItemStatus InProgress { get; } = new("in_progress");

    /// <summary>
    ///     The item has been completed successfully.
    /// </summary>
    public static ItemStatus Completed { get; } = new("completed");

    /// <summary>
    ///     The item was not completed due to reaching limits or other constraints.
    /// </summary>
    public static ItemStatus Incomplete { get; } = new("incomplete");

    /// <summary>
    ///     The item is currently searching (specific to search operations).
    /// </summary>
    public static ItemStatus Searching { get; } = new("searching");

    /// <summary>
    ///     The item failed to complete.
    /// </summary>
    public static ItemStatus Failed { get; } = new("failed");

    /// <summary>
    ///     The underlying string value of the status.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(ItemStatus other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is ItemStatus other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="ItemStatus" /> values are equal.
    /// </summary>
    public static bool operator ==(ItemStatus left, ItemStatus right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="ItemStatus" /> values are not equal.
    /// </summary>
    public static bool operator !=(ItemStatus left, ItemStatus right) => !(left == right);

    /// <summary>
    ///     Implicitly converts an <see cref="ItemStatus" /> to a string.
    /// </summary>
    public static implicit operator string(ItemStatus status) => status.Value;

    /// <summary>
    ///     Implicitly converts a string to an <see cref="ItemStatus" />.
    /// </summary>
    public static implicit operator ItemStatus(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="ItemStatus" />.
    /// </summary>
    public sealed class Converter : JsonConverter<ItemStatus>
    {
        /// <inheritdoc />
        public override ItemStatus Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, ItemStatus value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

