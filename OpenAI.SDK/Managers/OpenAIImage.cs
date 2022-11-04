using OpenAI.GPT3.Extensions;
using OpenAI.GPT3.Interfaces;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels.ImageResponseModel;

namespace OpenAI.GPT3.Managers;

public partial class OpenAIService : IImageService
{

    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    /// <param name="imageCreateModel"></param>
    /// <returns></returns>
    public async Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreateModel)
    {
        return await _httpClient.PostAndReadAsAsync<ImageCreateResponse>(_endpointProvider.ImageCreate(), imageCreateModel);
    }
}