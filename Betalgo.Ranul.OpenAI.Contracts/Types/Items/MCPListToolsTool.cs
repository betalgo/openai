using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A tool available on an MCP server.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">MCP Tool Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcplisttoolstool.yml">
///         Source Definition
///     </see>
/// </summary>
public class MCPListToolsTool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPListToolsTool" /> class.
    /// </summary>
    public MCPListToolsTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPListToolsTool" /> class with required parameters.
    /// </summary>
    /// <param name="name">The name of the tool.</param>
    /// <param name="inputSchema">The JSON schema describing the tool's input.</param>
    public MCPListToolsTool(string name, Dictionary<string, object> inputSchema)
    {
        Name = name;
        InputSchema = inputSchema;
    }

    /// <summary>
    ///     The name of the tool.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     The description of the tool.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     The JSON schema describing the tool's input.
    /// </summary>
    [JsonPropertyName("input_schema")]
    public Dictionary<string, object> InputSchema { get; set; } = null!;

    /// <summary>
    ///     Additional annotations about the tool.
    /// </summary>
    [JsonPropertyName("annotations")]
    public Dictionary<string, object>? Annotations { get; set; }
}

