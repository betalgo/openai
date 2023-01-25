using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels;

namespace OpenAI.GPT3.Interfaces
{
    /// <summary>
    ///     Given a input text, outputs if the model classifies it as violating OpenAI's content policy.
    ///     Related guide: <a href="https://beta.openai.com/docs/guides/moderation">Moderations</a>
    /// </summary>
    public interface IModerationService
    {
        /// <summary>
        ///     Classifies if text violates OpenAI's Content Policy
        /// </summary>
        /// <param name="createModerationRequest"></param>
        /// <returns></returns>
        Task<CreateModerationResponse> CreateModeration(CreateModerationRequest createModerationRequest);

    }

    public static class IModerationServiceExtension
    {
        /// <summary>
        ///     Classifies if text violates OpenAI's Content Policy
        /// </summary>
        /// <param name="service"></param>
        /// <param name="input">The input text to classify</param>
        /// <param name="model">
        ///     Two content moderations models are available: text-moderation-stable and text-moderation-latest.
        ///     The default is text-moderation-latest which will be automatically upgraded over time. This ensures you are always
        ///     using our most accurate model. If you use text-moderation-stable, we will provide advanced notice before updating
        ///     the model. Accuracy of text-moderation-stable may be slightly lower than for text-moderation-latest.
        /// </param>
        /// <returns></returns>
        public static Task<CreateModerationResponse> CreateModeration(this IModerationService service, string input, string? model = null)
        {
            return service.CreateModeration(new CreateModerationRequest()
            {
                Input = input,
                Model = model
            });
        }
    }
}