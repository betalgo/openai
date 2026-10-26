using System.Text.Json;
using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

/// <summary>
///     Identifier for service connectors available in ChatGPT.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp#connectors">MCP Connectors Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcptool.yml">
///         Source Definition
///     </see>
/// </summary>
[JsonConverter(typeof(Converter))]
public readonly struct MCPConnectorId(string value) : IEquatable<MCPConnectorId>
{
    /// <summary>
    ///     Dropbox connector.
    /// </summary>
    public static MCPConnectorId Dropbox { get; } = new("connector_dropbox");

    /// <summary>
    ///     Gmail connector.
    /// </summary>
    public static MCPConnectorId Gmail { get; } = new("connector_gmail");

    /// <summary>
    ///     Google Calendar connector.
    /// </summary>
    public static MCPConnectorId GoogleCalendar { get; } = new("connector_googlecalendar");

    /// <summary>
    ///     Google Drive connector.
    /// </summary>
    public static MCPConnectorId GoogleDrive { get; } = new("connector_googledrive");

    /// <summary>
    ///     Microsoft Teams connector.
    /// </summary>
    public static MCPConnectorId MicrosoftTeams { get; } = new("connector_microsoftteams");

    /// <summary>
    ///     Outlook Calendar connector.
    /// </summary>
    public static MCPConnectorId OutlookCalendar { get; } = new("connector_outlookcalendar");

    /// <summary>
    ///     Outlook Email connector.
    /// </summary>
    public static MCPConnectorId OutlookEmail { get; } = new("connector_outlookemail");

    /// <summary>
    ///     SharePoint connector.
    /// </summary>
    public static MCPConnectorId SharePoint { get; } = new("connector_sharepoint");

    /// <summary>
    ///     The underlying string value of the connector ID.
    /// </summary>
    public string Value { get; } = value;

    /// <inheritdoc />
    public override string ToString() => Value;

    /// <inheritdoc />
    public bool Equals(MCPConnectorId other) => string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is MCPConnectorId other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => StringComparer.OrdinalIgnoreCase.GetHashCode(Value);

    /// <summary>
    ///     Determines whether two <see cref="MCPConnectorId" /> values are equal.
    /// </summary>
    public static bool operator ==(MCPConnectorId left, MCPConnectorId right) => left.Equals(right);

    /// <summary>
    ///     Determines whether two <see cref="MCPConnectorId" /> values are not equal.
    /// </summary>
    public static bool operator !=(MCPConnectorId left, MCPConnectorId right) => !(left == right);

    /// <summary>
    ///     Implicitly converts a <see cref="MCPConnectorId" /> to a string.
    /// </summary>
    public static implicit operator string(MCPConnectorId connector) => connector.Value;

    /// <summary>
    ///     Implicitly converts a string to a <see cref="MCPConnectorId" />.
    /// </summary>
    public static implicit operator MCPConnectorId(string value) => new(value);

    /// <summary>
    ///     JSON converter for <see cref="MCPConnectorId" />.
    /// </summary>
    public sealed class Converter : JsonConverter<MCPConnectorId>
    {
        /// <inheritdoc />
        public override MCPConnectorId Read(ref Utf8JsonReader reader, Type t, JsonSerializerOptions o) => new(reader.GetString()!);

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, MCPConnectorId value, JsonSerializerOptions o) => writer.WriteStringValue(value.Value);
    }
}

