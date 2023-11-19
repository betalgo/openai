using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Interfaces;

/// <summary>
///     Given a chat conversation, the model will return a chat completion response.
/// </summary>
public interface IChatCompletionService
{
    /// <summary>
    ///     Creates a completion for the chat message
    /// </summary>
    /// <param name="modelId">ID of the model to use. Currently, only gpt-3.5-turbo and gpt-3.5-turbo-0301 are supported.</param>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ChatCompletionCreateResponse> CreateCompletion(ChatCompletionCreateRequest chatCompletionCreate, string? modelId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters and returns a stream of CompletionCreateRequests
    /// </summary>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    IAsyncEnumerable<ChatCompletionCreateResponse> CreateCompletionAsStream(ChatCompletionCreateRequest chatCompletionCreate, string? modelId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a completion for the Vision API chat message
    ///     GPT-4 with Vision, sometimes referred to as GPT-4V or gpt-4-vision-preview in the API, allows the model to take in images and answer questions about them.
    ///     Images are made available to the model in two main ways:
    ///     by passing a link to the image or by passing the base64 encoded image directly in the request.
    ///     Images can be passed in the user, system and assistant messages. Currently openAI doesn't support images in the first system message.
    ///     The Chat Completions API is not stateful. That means you have to manage the messages (including images) you pass to the model yourself.
    ///     If you want to pass the same image to the model multiple times, you will have to pass the image each time you make a request to the API.
    /// </summary>
    /// <param name="modelId">ID of the model to use. Currently, only gpt-4-vision-preview is supported.</param>
    /// <param name="visionCreate"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ChatCompletionCreateResponse> CreateVision(VisionCreateRequest visionCreate, string? modelId = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a completion for the Vision API chat message and returns a stream of VisionCreateRequest
    /// </summary>
    /// <param name="modelId">ID of the model to use. Currently, only gpt-4-vision-preview is supported.</param>
    /// <param name="visionCreate"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    IAsyncEnumerable<ChatCompletionCreateResponse> CreateVisionAsStream(VisionCreateRequest visionCreate, string? modelId = null, CancellationToken cancellationToken = default);

}

public static class IChatCompletionServiceExtension
{
    /// <summary>
    ///     Creates a new completion for the provided prompt and parameters
    /// </summary>
    /// <param name="service"></param>
    /// <param name="chatCompletionCreate"></param>
    /// <param name="modelId">The ID of the model to use for this request</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    public static Task<ChatCompletionCreateResponse> Create(this IChatCompletionService service, ChatCompletionCreateRequest chatCompletionCreate, Models.Model modelId, CancellationToken cancellationToken = default)
    {
        return service.CreateCompletion(chatCompletionCreate, modelId.EnumToString(), cancellationToken);
    }
}