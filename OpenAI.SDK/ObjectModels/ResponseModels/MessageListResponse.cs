using System.Text.Json.Serialization;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.ObjectModels.ResponseModels;

public record MessageListResponse : DataWithPagingBaseResponse<List<MessageResponse>>
{
}
