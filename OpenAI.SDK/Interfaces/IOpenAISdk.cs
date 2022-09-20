namespace OpenAI.GPT3.Interfaces;

public interface IOpenAIService
{
    /// <summary>
    ///     List and describe the various models available in the API. You can refer to the
    ///     <a href="https://beta.openai.com/docs/models">Models</a> documentation to understand what models are available and
    ///     the differences between them.
    /// </summary>
    public IModel Models { get; }

    /// <summary>
    ///     Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of
    ///     alternative tokens at each position.
    /// </summary>
    public ICompletion Completions { get; }

    /// <summary>
    ///     Creates an embedding vector representing the input text.
    /// </summary>
    public IEmbedding Embeddings { get; }

    /// <summary>
    ///     Files are used to upload documents that can be used across features like <see cref="FineTunes" />
    /// </summary>
    public IFile Files { get; }

    public IFineTune FineTunes { get; }

    /// <summary>
    ///     Set default engine
    /// </summary>
    /// <param name="engineId"></param>
    void SetDefaultEngineId(string engineId);
}