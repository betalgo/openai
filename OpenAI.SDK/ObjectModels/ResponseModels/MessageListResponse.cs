using Betalgo.OpenAI.ObjectModels.SharedModels;

namespace Betalgo.OpenAI.ObjectModels.ResponseModels;

public record MessageListResponse : DataWithPagingBaseResponse<List<MessageResponse>>
{
}