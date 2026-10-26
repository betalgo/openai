using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     The status of an MCP tool call.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">MCP Tool Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcptoolcallstatus.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct MCPToolCallStatus(string value) : IEquatable<MCPToolCallStatus>
{
    /// <summary>
    ///     The tool call is in progress.
    /// </summary>
    public static MCPToolCallStatus InProgress { get; } = new("in_progress");

    /// <summary>
    ///     The tool call has been completed successfully.
    /// </summary>
    public static MCPToolCallStatus Completed { get; } = new("completed");

    /// <summary>
    ///     The tool call was not completed.
    /// </summary>
    public static MCPToolCallStatus Incomplete { get; } = new("incomplete");

    /// <summary>
    ///     The tool is currently being called.
    /// </summary>
    public static MCPToolCallStatus Calling { get; } = new("calling");

    /// <summary>
    ///     The tool call failed.
    /// </summary>
    public static MCPToolCallStatus Failed { get; } = new("failed");

    /// <summary>
    ///     The underlying string value of the status.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(MCPToolCallStatus other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MCPToolCallStatus other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="MCPToolCallStatus" /> values are equal.
    /// </summary>
    public static bool operator ==(MCPToolCallStatus left, MCPToolCallStatus right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="MCPToolCallStatus" /> values are not equal.
    /// </summary>
    public static bool operator !=(MCPToolCallStatus left, MCPToolCallStatus right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="MCPToolCallStatus" /> to a string.
    /// </summary>
    public static implicit operator string(MCPToolCallStatus status) => status.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="MCPToolCallStatus" />.
    /// </summary>
    public static implicit operator MCPToolCallStatus(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="MCPToolCallStatus" />.
    /// </summary>
    public sealed class Converter : JsonConverter<MCPToolCallStatus>
    {
        /// <inheritdoc />
        public override MCPToolCallStatus Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MCPToolCallStatus value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

