using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     An action taken during a web search.
///     <see href="https://platform.openai.com/docs/guides/tools-web-search">Web Search Guide</see>.
/// </summary>
public class WebSearchAction
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchAction" /> class.
    /// </summary>
    public WebSearchAction()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="type">The type of action.</param>
    /// <param name="query">The search query.</param>
    public WebSearchAction(string type, string? query = null)
    {
        Type = type;
        Query = query;
    }

    /// <summary>
    ///     The type of action. One of <c>search</c>, <c>open_page</c>, or <c>find</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    ///     The search query (for search actions).
    /// </summary>
    [JsonPropertyName("query")]
    public string? Query { get; set; }

    /// <summary>
    ///     The URL being accessed (for open_page actions).
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    ///     The pattern to find (for find actions).
    /// </summary>
    [JsonPropertyName("pattern")]
    public string? Pattern { get; set; }

    /// <summary>
    ///     The sources returned by the search (when include contains web_search_call.action.sources).
    /// </summary>
    [JsonPropertyName("sources")]
    public List<WebSearchSource>? Sources { get; set; }
}

