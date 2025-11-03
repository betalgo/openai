using Betalgo.Ranul.OpenAI.Contracts.Requests.Image;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Image;

namespace Betalgo.Ranul.OpenAI.Interfaces;

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
    Task<ImageResponse> CreateImage(CreateImageRequest imageCreate, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates an edited or extended image given an original image and a prompt.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageResponse> CreateImageEdit(CreateImageEditRequest imageEditCreateRequest, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a variation of a given image.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
    /// <returns></returns>
    Task<ImageResponse> CreateImageVariation(CreateImageVariationRequest imageEditCreateRequest, CancellationToken cancellationToken = default);
}