using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.ObjectModels.RequestModels;
using Betalgo.Ranul.OpenAI.ObjectModels.ResponseModels;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IModerationService
{
    /// <inheritdoc />
    public async Task<CreateModerationResponse> CreateModeration(CreateModerationRequest createModerationRequest, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAndReadAsAsync<CreateModerationResponse>(_endpointProvider.ModerationCreate(), createModerationRequest, cancellationToken);
    }
}