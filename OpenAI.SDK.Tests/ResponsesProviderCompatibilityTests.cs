using Betalgo.Ranul.OpenAI;
using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.Interfaces;
using Betalgo.Ranul.OpenAI.Managers;

namespace OpenAI.SDK.Tests;

public class ResponsesProviderCompatibilityTests
{
    [Fact]
    public void ConcreteAndInterfaceClientsReachTheSameResponsesService()
    {
        using var concrete = new OpenAIService(new OpenAIOptions { ApiKey = "fixture-only" });
        IOpenAIService service = concrete;
        Assert.Same(concrete.Responses, service.AsResponses());
        Assert.Null(typeof(IOpenAIService).GetProperty("Responses"));
    }

    [Fact]
    public void LegacyImplementerCompilesAndKeepsExistingCapabilities()
    {
        using var concrete = new OpenAIService(new OpenAIOptions { ApiKey = "fixture-only" });
        IOpenAIService decorated = new LegacyDecorator(concrete);
        Assert.Same(concrete.Models, decorated.Models);
        Assert.Throws<NotSupportedException>(() => decorated.AsResponses());
    }

    [Fact]
    public void NullClientFailsClearly() => Assert.Throws<ArgumentNullException>(() => ((IOpenAIService)null!).AsResponses());

    // This intentionally implements only the original dev-branch interface.
#pragma warning disable CS0618
    private sealed class LegacyDecorator(IOpenAIService inner) : IOpenAIService
    {
        public IModelService Models => inner.Models;
        public ICompletionService Completions => inner.Completions;
        public IEmbeddingService Embeddings => inner.Embeddings;
        public IFileService Files => inner.Files;
        public IFineTuneService FineTunes => inner.FineTunes;
        public IFineTuningJobService FineTuningJob => inner.FineTuningJob;
        public IModerationService Moderation => inner.Moderation;
        public IImageService Image => inner.Image;
        public IEditService Edit => inner.Edit;
        public IChatCompletionService ChatCompletion => inner.ChatCompletion;
        public IAudioService Audio => inner.Audio;
        public IBatchService Batch => inner.Batch;
        public IBetaService Beta => inner.Beta;
        public void SetDefaultModelId(string modelId) => inner.SetDefaultModelId(modelId);
    }
#pragma warning restore CS0618
}
