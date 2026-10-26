using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The order to return input items in for list operations.
///     <see href="https://platform.openai.com/docs/api-reference/responses/list-input-items">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/paths/responses/param-response_id/input_items/get/listinputitems.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct InputItemsOrder(string value) : IEquatable<InputItemsOrder>
{
    /// <summary>
    ///     Return the input items in ascending order.
    /// </summary>
    public static InputItemsOrder Asc { get; } = new("asc");

    /// <summary>
    ///     Return the input items in descending order.
    /// </summary>
    public static InputItemsOrder Desc { get; } = new("desc");

    /// <summary>
    ///     The underlying string value of the order option.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(InputItemsOrder other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is InputItemsOrder other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="InputItemsOrder" /> values are equal.
    /// </summary>
    public static bool operator ==(InputItemsOrder left, InputItemsOrder right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="InputItemsOrder" /> values are not equal.
    /// </summary>
    public static bool operator !=(InputItemsOrder left, InputItemsOrder right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="InputItemsOrder" /> to a string.
    /// </summary>
    public static implicit operator string(InputItemsOrder order) => order.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="InputItemsOrder" />.
    /// </summary>
    public static implicit operator InputItemsOrder(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="InputItemsOrder" />.
    /// </summary>
    public sealed class Converter : JsonConverter<InputItemsOrder>
    {
        /// <inheritdoc />
        public override InputItemsOrder Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, InputItemsOrder value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

