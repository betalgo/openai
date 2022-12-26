using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IEditService
{
    public async Task<EditCreateResponse> CreateEdit(EditCreateRequest editCreate, string? engineId = null)
    {
        if (editCreate.Model != null && engineId != null)
        {
            throw new ArgumentException("You cannot specify both a model and an engineId");
        }
        else if (editCreate.Model == null && engineId != null)
        {
            editCreate.Model = ProcessEngineId(engineId);
        }

        return await _httpClient.PostAndReadAsAsync<EditCreateResponse>(_endpointProvider.EditCreate(), editCreate);
    }
}