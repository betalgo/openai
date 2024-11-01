using Betalgo.Ranul.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

public record AssistantFileListResponse : DataWithPagingBaseResponse<List<AssistantFileResponse>>
{
}