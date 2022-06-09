using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Interfaces;

/// <summary>
///     Given a query and a set of labeled examples, the model will predict the most likely label for the query. Useful as
///     a drop-in replacement for any ML classification or text-to-label task.
///     Related guide: <a href="https://beta.openai.com/docs/guides/classifications">Classify text</a>
/// </summary>
public interface IClassification
{
    /// <summary>
    ///     Classifies the specified query using provided examples.
    ///     The endpoint first searches over the labeled examples to select the ones most relevant for the particular
    ///     query.Then, the relevant examples are combined with the query to construct a prompt to produce the final label via
    ///     the completions endpoint.
    ///     Labeled examples can be provided via an uploaded file, or explicitly listed in the request using the examples
    ///     parameter for quick tests and small scale use cases.
    /// </summary>
    /// <param name="createClassificationRequest"></param>
    /// <returns></returns>
    Task<ClassificationCreateResponse> ClassificationsCreate(ClassificationCreateRequest createClassificationRequest);
}