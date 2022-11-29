using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IEditService
{
    public async Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? engineId = null)
    {
        editCreate.Model = ProcessEngineId(engineId);
        return await _httpClient.PostAndReadAsAsync<EditCreateResponse>(_endpointProvider.EditCreate(), editCreate);
    }
}