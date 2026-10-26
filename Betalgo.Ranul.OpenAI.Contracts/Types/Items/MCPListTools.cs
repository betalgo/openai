using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A list of tools available on an MCP server.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">MCP Tool Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcplisttools.yml">
///         Source Definition
///     </see>
/// </summary>
public class MCPListTools : IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPListTools" /> class.
    /// </summary>
    public MCPListTools()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPListTools" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the list.</param>
    /// <param name="serverLabel">The label of the MCP server.</param>
    /// <param name="tools">The tools available on the server.</param>
    public MCPListTools(string id, string serverLabel, List<MCPListToolsTool> tools)
    {
        Id = id;
        ServerLabel = serverLabel;
        Tools = tools;
    }

    /// <summary>
    ///     The type of the item. Always <c>mcp_list_tools</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "mcp_list_tools";

    /// <summary>
    ///     The unique ID of the list.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The label of the MCP server.
    /// </summary>
    [JsonPropertyName("server_label")]
    public string ServerLabel { get; set; } = null!;

    /// <summary>
    ///     The tools available on the server.
    /// </summary>
    [JsonPropertyName("tools")]
    public List<MCPListToolsTool> Tools { get; set; } = null!;

    /// <summary>
    ///     Error message if the server could not list tools.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }
}

