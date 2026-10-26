using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;

/// <summary>
///     Details about why the response is incomplete.
///     <see href="https://platform.openai.com/docs/api-reference/responses/object">OpenAI API documentation</see>.
/// </summary>
public class IncompleteDetails
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="IncompleteDetails" /> class.
    /// </summary>
    public IncompleteDetails()
    {
    }

    /// <summary>
    ///     Initializes a new instance with a reason.
    /// </summary>
    /// <param name="reason">The reason why the response is incomplete.</param>
    public IncompleteDetails(string reason)
    {
        Reason = reason;
    }

    /// <summary>
    ///     The reason why the response is incomplete.
    ///     One of <c>max_output_tokens</c> or <c>content_filter</c>.
    /// </summary>
    [JsonPropertyName("reason")]
    public string? Reason { get; set; }
}

