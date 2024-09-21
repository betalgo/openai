using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record ChatCompletionCreateResponse : BaseResponse, IOpenAiModels.IId, IOpenAiModels.ICreatedAt
{
    /// <summary>
    ///     The model used for the chat completion.
    /// </summary>
    [JsonPropertyName("model")]
    public string Model { get; set; }

    /// <summary>
    ///     A list of chat completion choices. Can be more than one if n is greater than 1.
    /// </summary>
    [JsonPropertyName("choices")]
    public List<ChatChoiceResponse> Choices { get; set; }

    /// <summary>
    ///     Usage statistics for the completion request.
    /// </summary>
    [JsonPropertyName("usage")]
    public UsageResponse Usage { get; set; }

    /// <summary>
    ///     This fingerprint represents the backend configuration that the model runs with.
    ///     Can be used in conjunction with the seed request parameter to understand when backend changes have been made that
    ///     might impact determinism.
    /// </summary>
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerPrint { get; set; }

    /// <summary>
    ///     The service tier used for processing the request. This field is only included if the service_tier parameter is
    ///     specified in the request.
    /// </summary>
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }

    /// <summary>
    ///     The Unix timestamp (in seconds) of when the chat completion was created.
    /// </summary>
    [JsonPropertyName("created")]
    public int CreatedAt { get; set; }

    /// <summary>
    ///     A unique identifier for the chat completion.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
}