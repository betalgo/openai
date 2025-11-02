using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Responses;

/// <summary>
///     For <c>gpt-image-1</c> only, the token usage information for the image generation.
/// </summary>
public class UsageResponse
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UsageResponse" /> class.
    /// </summary>
    public UsageResponse()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UsageResponse" /> class.
    /// </summary>
    /// <param name="totalTokens">
    ///     The total number of tokens (images and text) used for the image generation.
    /// </param>
    /// <param name="inputTokens">
    ///     The number of tokens (images and text) in the input prompt.
    /// </param>
    /// <param name="outputTokens">
    ///     The number of image tokens in the output image.
    /// </param>
    /// <param name="inputTokensDetails">
    ///     The input tokens detailed information for the image generation.
    /// </param>
    public UsageResponse(int totalTokens, int inputTokens, int outputTokens, InputTokensDetailsResponse inputTokensDetails)
    {
        TotalTokens = totalTokens;
        InputTokens = inputTokens;
        OutputTokens = outputTokens;
        InputTokensDetails = inputTokensDetails;
    }

    /// <summary>
    ///     The total number of tokens (images and text) used for the image generation.
    /// </summary>
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }

    /// <summary>
    ///     The number of tokens (images and text) in the input prompt.
    /// </summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    /// <summary>
    ///     The number of image tokens in the output image.
    /// </summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }

    /// <summary>
    ///     The input tokens detailed information for the image generation.
    /// </summary>
    [JsonPropertyName("input_tokens_details")]
    public InputTokensDetailsResponse InputTokensDetails { get; set; }


    /// <summary>
    ///     The input tokens detailed information for the image generation.
    /// </summary>
    public class InputTokensDetailsResponse
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="InputTokensDetailsResponse" /> class.
        /// </summary>
        public InputTokensDetailsResponse()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputTokensDetailsResponse" /> class.
        /// </summary>
        /// <param name="textTokens">
        ///     The number of text tokens in the input prompt.
        /// </param>
        /// <param name="imageTokens">
        ///     The number of image tokens in the input prompt.
        /// </param>
        public InputTokensDetailsResponse(int textTokens, int imageTokens)
        {
            TextTokens = textTokens;
            ImageTokens = imageTokens;
        }

        /// <summary>
        ///     The number of text tokens in the input prompt.
        /// </summary>
        [JsonPropertyName("text_tokens")]
        public int TextTokens { get; set; }

        /// <summary>
        ///     The number of image tokens in the input prompt.
        /// </summary>
        [JsonPropertyName("image_tokens")]
        public int ImageTokens { get; set; }
    }
}