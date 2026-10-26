using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     Filters for the web search tool.
///     <see href="https://platform.openai.com/docs/guides/tools-web-search">Web Search Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/websearchtool.yml">
///         Source Definition
///     </see>
/// </summary>
public class WebSearchFilters
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchFilters" /> class.
    /// </summary>
    public WebSearchFilters()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchFilters" /> class with allowed domains.
    /// </summary>
    /// <param name="allowedDomains">The list of allowed domains for the search.</param>
    public WebSearchFilters(List<string>? allowedDomains)
    {
        AllowedDomains = allowedDomains;
    }

    /// <summary>
    ///     Allowed domains for the search. If not provided, all domains are allowed.
    ///     Subdomains of the provided domains are allowed as well.
    ///     Example: <c>["pubmed.ncbi.nlm.nih.gov"]</c>
    /// </summary>
    [JsonPropertyName("allowed_domains")]
    public List<string>? AllowedDomains { get; set; }
}

