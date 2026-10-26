using Betalgo.Ranul.OpenAI.Contracts.Requests.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Interfaces;

namespace OpenAI.IntegrationTests.Helpers;

/// <summary>Records created resources before assertions and deletes all of them when a test exits.</summary>
internal sealed class StoredResponseScope(IResponsesService service) : IAsyncDisposable
{
    private readonly List<string> _ids = [];

    public async Task<Response> Create(CreateResponse request)
    {
        request.Store = true;
        var result = await service.CreateResponse(request);
        if (!string.IsNullOrWhiteSpace(result.Id) && !_ids.Contains(result.Id)) _ids.Add(result.Id);
        return result;
    }

    public void Forget(string id) => _ids.Remove(id);

    public async ValueTask DisposeAsync()
    {
        using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(15));
        var failures = new List<Exception>();
        foreach (var id in _ids.AsEnumerable().Reverse())
        {
            try
            {
                var result = await service.DeleteResponse(id, timeout.Token);
                if (!result.Successful || !result.Deleted)
                    failures.Add(new InvalidOperationException($"Stored test response cleanup failed: {result.Error?.Code ?? "not_deleted"}."));
            }
            catch (Exception error) { failures.Add(error); }
        }
        if (failures.Count > 0) throw new AggregateException("Stored test responses could not all be cleaned up.", failures);
    }
}
