using Betalgo.OpenAI.Extensions;
using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.ObjectModels.RequestModels;
using Betalgo.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.OpenAI.Managers;

public partial class OpenAIService : IEditService
{
    public async Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? modelId = null, CancellationToken cancellationToken = default)
    {
        editCreate.ProcessModelId(modelId, _defaultModelId);
        return await _httpClient.PostAndReadAsAsync<EditCreateResponse>(_endpointProvider.EditCreate(), editCreate, cancellationToken);
    }
}