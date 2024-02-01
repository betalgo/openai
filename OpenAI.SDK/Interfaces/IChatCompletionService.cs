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
    /// <param name="justDataMode">Ignore stream lines if they don’t start with "data:". If you don't know what it means, probably you shouldn't change this.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    IAsyncEnumerable<ChatCompletionCreateResponse> CreateCompletionAsStream(ChatCompletionCreateRequest chatCompletionCreate, string? modelId = null, bool justDataMode = true,CancellationToken cancellationToken = default);
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