using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     Give the model access to additional tools via remote Model Context Protocol (MCP) servers.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">Learn more about MCP</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcptool.yml">
///         Source Definition
///     </see>
/// </summary>
public class MCPTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPTool" /> class.
    /// </summary>
    public MCPTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPTool" /> class with required parameters.
    /// </summary>
    /// <param name="serverLabel">A label for this MCP server, used to identify it in tool calls.</param>
    public MCPTool(string serverLabel)
    {
        ServerLabel = serverLabel;
    }

    /// <summary>
    ///     The type of the MCP tool. Always <c>mcp</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "mcp";

    /// <summary>
    ///     A label for this MCP server, used to identify it in tool calls.
    /// </summary>
    [JsonPropertyName("server_label")]
    public string ServerLabel { get; set; } = null!;

    /// <summary>
    ///     The URL for the MCP server. One of <c>server_url</c> or <c>connector_id</c> must be provided.
    /// </summary>
    [JsonPropertyName("server_url")]
    public string? ServerUrl { get; set; }

    /// <summary>
    ///     Identifier for service connectors, like those available in ChatGPT.
    ///     One of <c>server_url</c> or <c>connector_id</c> must be provided.
    /// </summary>
    [JsonPropertyName("connector_id")]
    public MCPConnectorId? ConnectorId { get; set; }

    /// <summary>
    ///     An OAuth access token that can be used with a remote MCP server.
    /// </summary>
    [JsonPropertyName("authorization")]
    public string? Authorization { get; set; }

    /// <summary>
    ///     Optional description of the MCP server, used to provide more context.
    /// </summary>
    [JsonPropertyName("server_description")]
    public string? ServerDescription { get; set; }

    /// <summary>
    ///     Optional HTTP headers to send to the MCP server. Use for authentication or other purposes.
    /// </summary>
    [JsonPropertyName("headers")]
    public Dictionary<string, string>? Headers { get; set; }

    /// <summary>
    ///     List of allowed tool names or a filter object.
    /// </summary>
    [JsonPropertyName("allowed_tools")]
    public object? AllowedTools { get; set; }

    /// <summary>
    ///     Specify which of the MCP server's tools require approval.
    ///     Can be <c>always</c>, <c>never</c>, or a filter object.
    /// </summary>
    [JsonPropertyName("require_approval")]
    public object? RequireApproval { get; set; }
}

