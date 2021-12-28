using OpenAI.SDK.Models.RequestModels;
using OpenAI.SDK.Models.ResponseModels;

namespace OpenAI.SDK.Interfaces;

/// <summary>
///     Given a question, a set of documents, and some examples, the API generates an answer to the question based on the
///     information in the set of documents. This is useful for question-answering applications on sources of truth, like
///     company documentation or a knowledge base.
///     Related guide:<a href="https://beta.openai.com/docs/guides/answers">Answer questions</a>
/// </summary>
public interface IAnswer
{
    /// <summary>
    ///     Answers the specified question using the provided documents and examples.
    ///     The endpoint first searches over provided documents or files to find relevant context.The relevant context is
    ///     combined with the provided examples and question to create the prompt for completion.
    /// </summary>
    /// <param name="createAnswerRequest"></param>
    /// <returns></returns>
    Task<AnswerCreateResponse> Answer(AnswerCreateRequest createAnswerRequest);
}