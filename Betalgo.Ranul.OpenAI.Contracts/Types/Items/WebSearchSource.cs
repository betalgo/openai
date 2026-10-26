using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     A source returned by a web search.
///     <see href="https://platform.openai.com/docs/guides/tools-web-search">Web Search Guide</see>.
/// </summary>
public class WebSearchSource
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WebSearchSource" /> class.
    /// </summary>
    public WebSearchSource()
    {
    }

    /// <summary>
    ///     Initializes a new instance with parameters.
    /// </summary>
    /// <param name="url">The URL of the source.</param>
    /// <param name="title">The title of the source.</param>
    public WebSearchSource(string? url = null, string? title = null)
    {
        Url = url;
        Title = title;
    }

    /// <summary>
    ///     The URL of the source.
    /// </summary>
    [JsonPropertyName("url")]
    public string? Url { get; set; }

    /// <summary>
    ///     The title of the source.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }
}

