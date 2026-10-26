using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A request for human approval of a tool invocation.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">MCP Tool Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcpapprovalrequest.yml">
///         Source Definition
///     </see>
/// </summary>
public class MCPApprovalRequest : IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPApprovalRequest" /> class.
    /// </summary>
    public MCPApprovalRequest()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPApprovalRequest" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the approval request.</param>
    /// <param name="serverLabel">The label of the MCP server making the request.</param>
    /// <param name="name">The name of the tool to run.</param>
    /// <param name="arguments">A JSON string of arguments for the tool.</param>
    public MCPApprovalRequest(string id, string serverLabel, string name, string arguments)
    {
        Id = id;
        ServerLabel = serverLabel;
        Name = name;
        Arguments = arguments;
    }

    /// <summary>
    ///     The type of the item. Always <c>mcp_approval_request</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "mcp_approval_request";

    /// <summary>
    ///     The unique ID of the approval request.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The label of the MCP server making the request.
    /// </summary>
    [JsonPropertyName("server_label")]
    public string ServerLabel { get; set; } = null!;

    /// <summary>
    ///     The name of the tool to run.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     A JSON string of arguments for the tool.
    /// </summary>
    [JsonPropertyName("arguments")]
    public string Arguments { get; set; } = null!;
}

