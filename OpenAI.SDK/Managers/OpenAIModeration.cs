using Betalgo.OpenAI.Extensions;
using Betalgo.OpenAI.Interfaces;
using Betalgo.OpenAI.ObjectModels.RequestModels;
using Betalgo.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.OpenAI.Managers;

public partial class OpenAIService : IModerationService
{
    /// <inheritdoc />
    public async Task<CreateModerationResponse> CreateModeration(CreateModerationRequest createModerationRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<CreateModerationResponse>(_endpointProvider.ModerationCreate(), createModerationRequest, cancellationToken);
    }
}