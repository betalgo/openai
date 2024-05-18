using OpenAI.Interfaces;

namespace OpenAI.Managers;

/// <summary>
///     Beta service for OpenAI.
/// </summary>
public partial class OpenAIService : IBetaService
{
    public IAssistantService Assistants => this;

    public IMessageService Messages => this;

    public IThreadService Threads => this;

    public IRunService Runs => this;

    public IRunStepService RunSteps => this;

    public IVectorStores VectorStores => this;

    public IVectorStoreFiles VectorStoreFiles => this;
}