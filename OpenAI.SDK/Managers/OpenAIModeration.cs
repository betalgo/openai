using OpenAI.Extensions;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels;

namespace OpenAI.Managers;

public partial class OpenAIService : IModerationService
{
    /// <inheritdoc />
    public async Task<CreateModerationResponse> CreateModeration(CreateModerationRequest createModerationRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<CreateModerationResponse>(_endpointProvider.ModerationCreate(), createModerationRequest, cancellationToken);
    }
}