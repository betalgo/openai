namespace OpenAI.GPT3.Interfaces;

public interface IOpenAIService
{
    /// <summary>
    ///     Engines describe and provide access to the various models available in the API. You can refer to the
    ///     <a href="https://beta.openai.com/docs/engines">Engine</a> documentation to understand what engines are available
    ///     and the differences between them.
    /// </summary>
    public IEngine Engine { get; }

    /// <summary>
    ///     Given a prompt, the model will return one or more predicted completions, and can also return the probabilities of
    ///     alternative tokens at each position.
    /// </summary>
    public ICompletion Completions { get; }

    /// <summary>
    ///     Given a query and a set of documents or labels, the model ranks each document based on its semantic similarity to
    ///     the provided query.
    ///     Related guide: <a href="https://beta.openai.com/docs/guides/search">Search</a>
    /// </summary>
    public ISearch Searches { get; }

    /// <summary>
    ///     Given a query and a set of labeled examples, the model will predict the most likely label for the query. Useful as
    ///     a drop-in replacement for any ML classification or text-to-label task.
    ///     Related guide: <a href="https://beta.openai.com/docs/guides/classifications">Classify text</a>
    /// </summary>
    public IClassification Classifications { get; }

    /// <summary>
    ///     Given a question, a set of documents, and some examples, the API generates an answer to the question based on the
    ///     information in the set of documents. This is useful for question-answering applications on sources of truth, like
    ///     company documentation or a knowledge base.
    ///     Related guide:<a href="https://beta.openai.com/docs/guides/answers">Answer questions</a>
    /// </summary>
    public IAnswer Answers { get; }

    /// <summary>
    ///     Files are used to upload documents that can be used across features like <see cref="Answers" />,
    ///     <see cref="Searches" /> and <see cref="Classifications" />
    /// </summary>
    public IFile Files { get; }

    public IFineTune FineTunes { get; }

    /// <summary>
    ///     Set default engine
    /// </summary>
    /// <param name="engineId"></param>
    void SetDefaultEngineId(string engineId);
}