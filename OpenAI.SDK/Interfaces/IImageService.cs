using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels.ResponseModels.ImageResponseModel;

namespace OpenAI.GPT3.Interfaces;
/// <summary>
/// Given a prompt and/or an input image, the model will generate a new image.
/// Related guide: <a href="https://beta.openai.com/docs/guides/images">Image generation</a>
/// </summary>
public interface IImageService
{
    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    /// <param name="imageCreateModel"></param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreateModel);
    /// <summary>
    /// Creates an image given a prompt.
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImage(string prompt) => CreateImage(new ImageCreateRequest(prompt));
}