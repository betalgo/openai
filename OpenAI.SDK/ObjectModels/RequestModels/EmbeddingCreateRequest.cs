using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.RequestModels;

//TODO add model validation
//TODO check what is string or array for prompt,..
public record EmbeddingCreateRequest : IModelValidate, IOpenAiModels.IModel
{
    /// <summary>
    ///     Input text to get embeddings for, encoded as a string or array of tokens. To get embeddings for multiple inputs
    ///     in a single request, pass an array of strings or array of token arrays. Each input must not exceed 2048 tokens in
    ///     length.
    ///     Unless your are embedding code, we suggest replacing newlines (`\n`) in your input with a single space, as we have
    ///     observed inferior results when newlines are present.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/embeddings/create#embeddings/create-input" />
    [JsonIgnore]
    public List<string>? InputAsList { get; set; }

    /// <summary>
    ///     Input text to get embeddings for, encoded as a string or array of tokens. To get embeddings for multiple inputs
    ///     in a single request, pass an array of strings or array of token arrays. Each input must not exceed 2048 tokens in
    ///     length.
    ///     Unless your are embedding code, we suggest replacing newlines (`\n`) in your input with a single space, as we have
    ///     observed inferior results when newlines are present.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/embeddings/create#embeddings/create-input" />
    [JsonIgnore]
    public string? Input { get; set; }


    [JsonPropertyName("input")]
    public IList<string>? InputCalculated
    {
        get
        {
            if (Input != null && InputAsList != null)
            {
                throw new ValidationException("Input and InputAsList can not be assigned at the same time. One of them is should be null.");
            }

            if (Input != null)
            {
                return new List<string> {Input};
            }

            return InputAsList;
        }
    }

    /// <summary>
    ///     ID of the model to use. You can use the [List models](/docs/api-reference/models/list) API to see all of your
    ///     available models, or see our [Model overview](/docs/models/overview) for descriptions of them.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/embeddings/create#embeddings/create-model" />
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    /// <summary>
    ///     The number of dimensions the resulting output embeddings should have. Only supported in text-embedding-3 and later models.
    /// </summary>
    /// <see href="https://platform.openai.com/docs/api-reference/embeddings/create#embeddings-create-dimensions" />
    [JsonPropertyName("dimensions")]
    public int? Dimensions { get; set; }

    /// <summary>
    /// The format to return the embeddings in. Can be either float or base64.
    /// </summary>
    /// <returns></returns>
    [JsonPropertyName("encoding_format")]
    public string? EncodingFormat { get; set; }

    public IEnumerable<ValidationResult> Validate()
    {
        throw new NotImplementedException();
    }
}