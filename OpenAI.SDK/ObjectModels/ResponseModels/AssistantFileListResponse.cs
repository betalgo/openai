using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record AssistantFileListResponse : DataWithPagingBaseResponse<List<AssistantFileResponse>>
{
}