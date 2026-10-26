using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A web search tool that allows the model to search the web.
///     <see href="https://platform.openai.com/docs/guides/tools-web-search">Web Search Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/websearchtool.yml">
///         Source Definition
///     </see>
/// </summary>
public class WebSearchTool : ITool
{
    /// <summary>
    ///     Web search tool type.
    /// </summary>
    public const string TypeWebSearch = "web_search";

    /// <summary>
    ///     Web search tool type with version date.
    /// </summary>
    public const string TypeWebSearch20250826 = "web_search_2025_08_26";

    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchTool" /> class.
    /// </summary>
    public WebSearchTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchTool" /> class with parameters.
    /// </summary>
    /// <param name="type">The type of web search tool (web_search, web_search_2025_08_26).</param>
    public WebSearchTool(string type)
    {
        Type = type;
    }

    /// <summary>
    ///     The type of the web search tool. One of <c>web_search</c> or <c>web_search_2025_08_26</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = TypeWebSearch;

    /// <summary>
    ///     Filters for the search. Contains allowed domains configuration.
    /// </summary>
    [JsonPropertyName("filters")]
    public WebSearchFilters? Filters { get; set; }

    /// <summary>
    ///     Optional user location for the web search.
    /// </summary>
    [JsonPropertyName("user_location")]
    public UserLocation? UserLocation { get; set; }

    /// <summary>
    ///     High level guidance for the amount of context window space to use for the search.
    ///     One of <c>low</c>, <c>medium</c>, or <c>high</c>. <c>medium</c> is the default.
    /// </summary>
    [JsonPropertyName("search_context_size")]
    public string? SearchContextSize { get; set; }
}

