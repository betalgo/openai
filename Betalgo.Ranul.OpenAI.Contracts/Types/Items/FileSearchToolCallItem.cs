using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Responses;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Items;

/// <summary>
///     The results of a file search tool call. See the
///     <see href="https://platform.openai.com/docs/guides/tools-file-search">file search guide</see> for more information.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/components/schemas/filesearchtoolcall.yml">
///         Source Definition
///     </see>
/// </summary>
public class FileSearchToolCallItem : IInputItem, IOutputItem
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FileSearchToolCallItem" /> class.
    /// </summary>
    public FileSearchToolCallItem()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="FileSearchToolCallItem" /> class with required parameters.
    /// </summary>
    /// <param name="id">The unique ID of the file search tool call.</param>
    /// <param name="status">The status of the file search tool call.</param>
    /// <param name="queries">The queries used to search for files.</param>
    public FileSearchToolCallItem(string id, ItemStatus status, List<string> queries)
    {
        Id = id;
        Status = status;
        Queries = queries;
    }

    /// <summary>
    ///     The unique ID of the file search tool call.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     The type of the file search tool call. Always <c>file_search_call</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type => "file_search_call";

    /// <summary>
    ///     The status of the file search tool call. One of <c>in_progress</c>, <c>searching</c>, <c>incomplete</c> or
    ///     <c>failed</c>.
    /// </summary>
    [JsonPropertyName("status")]
    public ItemStatus Status { get; set; }

    /// <summary>
    ///     The queries used to search for files.
    /// </summary>
    [JsonPropertyName("queries")]
    public List<string> Queries { get; set; } = new();

    /// <summary>
    ///     The results of the file search tool call.
    /// </summary>
    [JsonPropertyName("results")]
    public List<FileSearchResult>? Results { get; set; }
}
