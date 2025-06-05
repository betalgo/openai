using System.Text.Json.Serialization;
using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

public record ImageCreateResponse : BaseResponse, IOpenAIModels.ICreatedAt
{
    [JsonPropertyName("data")]
    public List<ImageDataResult> Results { get; set; }

    [JsonPropertyName("created")]
    public long CreatedAtUnix { get; set; }  
    public DateTimeOffset CreatedAt => DateTimeOffset.FromUnixTimeSeconds(CreatedAtUnix);

    /// <summary>
    ///     For <c>gpt-image-1</c> only, the token usage information for the image generation.
    /// </summary>
    [JsonPropertyName("usage")]
    public UsageModel Usage { get; set; }

    public record ImageDataResult
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("b64_json")]
        public string B64 { get; set; }

        [JsonPropertyName("revised_prompt")]
        public string RevisedPrompt { get; set; }
    }
    /// <summary>
    ///     For <c>gpt-image-1</c> only, the token usage information for the image generation.
    ///
    /// </summary>
    public class UsageModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsageModel"/> class.
        /// </summary>
        public UsageModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsageModel" /> class.
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
        public UsageModel(int totalTokens, int inputTokens, int outputTokens, InputTokensDetailsModel inputTokensDetails)
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
        public InputTokensDetailsModel InputTokensDetails { get; set; }


        /// <summary>
        ///     The input tokens detailed information for the image generation.
        /// </summary>
        public class InputTokensDetailsModel
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="InputTokensDetailsModel"/> class.
            /// </summary>
            public InputTokensDetailsModel()
            {
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="InputTokensDetailsModel" /> class.
            /// </summary>
            /// <param name="textTokens">
            ///     The number of text tokens in the input prompt.
            /// </param>
            /// <param name="imageTokens">
            ///     The number of image tokens in the input prompt.
            /// </param>
            public InputTokensDetailsModel(int textTokens, int imageTokens)
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
}