using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels;

public record AssistantListResponse : DataWithPagingBaseResponse<List<AssistantResponse>>
{
}