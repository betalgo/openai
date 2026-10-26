using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A response to an MCP approval request.
///     <see href="https://platform.openai.com/docs/guides/tools-remote-mcp">MCP Tool Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/mcpapprovalresponse.yml">
///         Source Definition
///     </see>
/// </summary>
public class MCPApprovalResponse : IInputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPApprovalResponse" /> class.
    /// </summary>
    public MCPApprovalResponse()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MCPApprovalResponse" /> class with required parameters.
    /// </summary>
    /// <param name="approvalRequestId">The ID of the approval request being answered.</param>
    /// <param name="approve">Whether the request was approved.</param>
    public MCPApprovalResponse(string approvalRequestId, bool approve)
    {
        ApprovalRequestId = approvalRequestId;
        Approve = approve;
    }

    /// <summary>
    ///     The type of the item. Always <c>mcp_approval_response</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "mcp_approval_response";

    /// <summary>
    ///     The unique ID of the approval response.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    ///     The ID of the approval request being answered.
    /// </summary>
    [JsonPropertyName("approval_request_id")]
    public string ApprovalRequestId { get; set; } = null!;

    /// <summary>
    ///     Whether the request was approved.
    /// </summary>
    [JsonPropertyName("approve")]
    public bool Approve { get; set; }

    /// <summary>
    ///     Optional reason for the decision.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}

