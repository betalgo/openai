using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

public record AssistantListResponse : DataWithPagingBaseResponse<List<AssistantResponse>>
{
}