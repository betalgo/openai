using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IEditService
{
    public async Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? modelId = null, CancellationToken cancellationToken = default)
    {
        editCreate.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<EditCreateResponse>(_endpointProvider.EditCreate(), editCreate, cancellationToken);
    }
}