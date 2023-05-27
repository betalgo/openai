using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.ResponseModels.ImageResponseModel;

namespace OpenAI.Interfaces;

/// <summary>
///     Given a prompt and/or an input image, the model will generate a new image.
///     Related guide: <a href="https://platform.openai.com/docs/guides/images">Image generation</a>
/// </summary>
public interface IImageService
{
    /// <summary>
    ///     Creates an image given a prompt.
    /// </summary>
    /// <param name="imageCreate"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImage(ImageCreateRequest imageCreate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates an edited or extended image given an original image and a prompt.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImageEdit(ImageEditCreateRequest imageEditCreateRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a variation of a given image.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageCreateResponse> CreateImageVariation(ImageVariationCreateRequest imageEditCreateRequest, CancellationToken cancellationToken = default);
}

public static class IImageServiceExtension
{
    /// <summary>
    ///     Creates an image given a prompt.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="prompt"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    public static Task<ImageCreateResponse> CreateImage(this IImageService service, string prompt, CancellationToken cancellationToken = default)
    {
        return service.CreateImage(new ImageCreateRequest(prompt), cancellationToken);
    }
}