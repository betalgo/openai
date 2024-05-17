using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record AssistantListResponse : DataWithPagingBaseResponse<List<AssistantResponse>>
{
}