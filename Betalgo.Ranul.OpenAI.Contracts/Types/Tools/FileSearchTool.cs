using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     A file search tool that allows the model to search uploaded files.
///     <see href="https://platform.openai.com/docs/guides/tools-file-search">File Search Guide</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/filesearchtool.yml">
///         Source Definition
///     </see>
/// </summary>
public class FileSearchTool : ITool
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FileSearchTool" /> class.
    /// </summary>
    public FileSearchTool()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileSearchTool" /> class with vector store IDs.
    /// </summary>
    /// <param name="vectorStoreIds">The IDs of the vector stores to search.</param>
    public FileSearchTool(List<string> vectorStoreIds)
    {
        VectorStoreIds = vectorStoreIds;
    }

    /// <summary>
    ///     The type of the file search tool. Always <c>file_search</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "file_search";

    /// <summary>
    ///     The IDs of the vector stores to search.
    /// </summary>
    [JsonPropertyName("vector_store_ids")]
    public List<string>? VectorStoreIds { get; set; }

    /// <summary>
    ///     The maximum number of results to return.
    /// </summary>
    [JsonPropertyName("max_num_results")]
    public int? MaxNumResults { get; set; }

    /// <summary>
    ///     The ranking options for the search.
    /// </summary>
    [JsonPropertyName("ranking_options")]
    public RankingOptions? RankingOptions { get; set; }

    /// <summary>
    ///     Filters to apply to the search.
    /// </summary>
    [JsonPropertyName("filters")]
    public Dictionary<string, object>? Filters { get; set; }
}

