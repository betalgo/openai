namespace Betalgo.Ranul.OpenAI.Interfaces;

/// <summary>Optional Responses capability without adding members to existing IOpenAIService implementers.</summary>
public interface IResponsesProvider
{
    IResponsesService Responses { get; }
}
