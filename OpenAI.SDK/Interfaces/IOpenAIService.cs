namespace OpenAI.Interfaces;

public interface IOpenAIService
{
    /// <summary>
    ///     List and describe the various models available in the API. You can refer to the
    ///     <a href="https://platform.openai.com/docs/models">Models</a> documentation to understand what models are available
    ///     and
    ///     the differences between them.
    /// </summary>
    public IModelService Models { get; }

    /// <summary>
    ///     Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of
    ///     alternative tokens at each position.
    /// </summary>
    public ICompletionService Completions { get; }

    /// <summary>
    ///     Creates an embedding vector representing the input text.
    /// </summary>
    public IEmbeddingService Embeddings { get; }

    /// <summary>
    ///     Files are used to upload documents that can be used across features like <see cref="FineTunes" />
    /// </summary>
    public IFileService Files { get; }

    public IFineTuneService FineTunes { get; }

    /// <summary>
    /// Manage fine-tuning jobs to tailor a model to your specific training data.
    /// </summary>
    public IFineTuningJobService FineTuningJob { get; }

    public IModerationService Moderation { get; }

    /// <summary>
    ///     Given a prompt and/or an input image, the model will generate a new image.
    /// </summary>
    public IImageService Image { get; }

    /// <summary>
    ///     Creates a new edit for the provided input, instruction, and parameters
    /// </summary>
    public IEditService Edit { get; }

    /// <summary>
    ///     Given a chat conversation, the model will return a chat completion response.
    /// </summary>
    public IChatCompletionService ChatCompletion { get; }

    /// <summary>
    ///     Given an audio file, the model will return a transcription of the audio.
    /// </summary>
    public IAudioService Audio { get; }
    /// <summary>
    /// Create large batches of API requests to run asynchronously.
    /// </summary>
    public IBatchService Batch{ get; }

    /// <summary>
    ///  Beta
    /// </summary>
    public IBetaService Beta { get; }

    /// <summary>
    ///     Set default model
    /// </summary>
    /// <param name="modelId"></param>
    void SetDefaultModelId(string modelId);
}