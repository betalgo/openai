using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     An invocation of a tool on an MCP server.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">MCP Tool Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcptoolcall.yml">
///         Source Definition
///     </see>
/// </summary>
public class MCPToolCall : IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPToolCall" /> class.
    /// </summary>
    public MCPToolCall()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPToolCall" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the tool call.</param>
    /// <param name="serverLabel">The label of the MCP server running the tool.</param>
    /// <param name="name">The name of the tool that was run.</param>
    /// <param name="arguments">A JSON string of the arguments passed to the tool.</param>
    public MCPToolCall(string id, string serverLabel, string name, string arguments)
    {
        Id = id;
        ServerLabel = serverLabel;
        Name = name;
        Arguments = arguments;
    }

    /// <summary>
    ///     The type of the item. Always <c>mcp_call</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "mcp_call";

    /// <summary>
    ///     The unique ID of the tool call.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The label of the MCP server running the tool.
    /// </summary>
    [JsonPropertyName("server_label")]
    public string ServerLabel { get; set; } = null!;

    /// <summary>
    ///     The name of the tool that was run.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    ///     A JSON string of the arguments passed to the tool.
    /// </summary>
    [JsonPropertyName("arguments")]
    public string Arguments { get; set; } = null!;

    /// <summary>
    ///     The output from the tool call.
    /// </summary>
    [JsonPropertyName("output")]
    public string? Output { get; set; }

    /// <summary>
    ///     The error from the tool call, if any.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    ///     The status of the MCP tool call.
    /// </summary>
    [JsonPropertyName("status")]
    public MCPToolCallStatus? Status { get; set; }

    /// <summary>
    ///     Unique identifier for the MCP tool call approval request.
    ///     Include this value in a subsequent <c>mcp_approval_response</c> input to approve or reject the corresponding tool call.
    /// </summary>
    [JsonPropertyName("approval_request_id")]
    public string? ApprovalRequestId { get; set; }
}

