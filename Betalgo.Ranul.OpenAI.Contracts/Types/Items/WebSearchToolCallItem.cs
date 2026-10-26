using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     The results of a web search tool call. See the
///     <see href="https://platform.openai.com/docs/guides/tools-web-search">web search guide</see> for more information.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/websearchtoolcall.yml">
///         Source Definition
///     </see>
/// </summary>
public class WebSearchToolCallItem : IInputItem, IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchToolCallItem" /> class.
    /// </summary>
    public WebSearchToolCallItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchToolCallItem" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the web search tool call.</param>
    /// <param name="status">The status of the web search tool call.</param>
    /// <param name="action">The action taken in this web search call.</param>
    public WebSearchToolCallItem(string id, ItemStatus status, WebSearchAction action)
    {
        Id = id;
        Status = status;
        Action = action;
    }

    /// <summary>
    ///     The unique ID of the web search tool call.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The type of the web search tool call. Always <c>web_search_call</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "web_search_call";

    /// <summary>
    ///     The status of the web search tool call.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus Status { get; set; }

    /// <summary>
    ///     An object describing the specific action taken in this web search call.
    ///     Includes details on how the model used the web (search, open_page, find).
    /// </summary>
    [JsonPropertyName("action")]
    public WebSearchAction? Action { get; set; }
}
