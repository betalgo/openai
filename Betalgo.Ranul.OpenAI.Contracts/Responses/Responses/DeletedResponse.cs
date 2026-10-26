using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.Contracts.Interfaces;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Base;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     The response object returned when deleting a Response.
///     <see href="https://platform.openai.com/docs/api-reference/responses/delete">OpenAI API documentation</see>.
///     <see href="https://github.com/betalgo/openai/blob/master/Docs/openapi-split/paths/responses/param-response_id/delete/deleteresponse.yml">
///         Source Definition
///     </see>
/// </summary>
public class DeletedResponse : ResponseBase, IDefaultResult<DeletedResponse>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="DeletedResponse" /> class.
    /// </summary>
    public DeletedResponse()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="DeletedResponse" /> class.
    /// </summary>
    /// <param name="id">The ID of the deleted response.</param>
    /// <param name="deleted">Whether the response was deleted.</param>
    public DeletedResponse(string id, bool deleted)
    {
        Id = id;
        Deleted = deleted;
    }

    /// <summary>
    ///     Gets this instance as the result for <see cref="IDefaultResult{T}" />.
    /// </summary>
    [JsonIgnore]
    public DeletedResponse? Result => Successful ? this : null;

    /// <summary>
    ///     The ID of the deleted response.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    ///     Whether the response was successfully deleted.
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool Deleted { get; set; }
}

