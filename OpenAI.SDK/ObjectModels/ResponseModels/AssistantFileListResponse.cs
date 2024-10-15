using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels;

public record AssistantFileListResponse : DataWithPagingBaseResponse<List<AssistantFileResponse>>
{
}