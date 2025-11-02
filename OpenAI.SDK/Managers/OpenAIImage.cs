using System.Globalization;
using Betalgo.Ranul.OpenAI.Contracts.Requests.Image;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Image;
using Betalgo.Ranul.OpenAI.Extensions;
using Betalgo.Ranul.OpenAI.ExtensionsV2;
using Betalgo.Ranul.OpenAI.Interfaces;

namespace Betalgo.Ranul.OpenAI.Managers;

public partial class OpenAIService : IImageService
{
    /// <summary>
    ///     Creates an image given a prompt.
    /// </summary>
    /// <param name="imageCreateModel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ImageResponse> CreateImage(CreateImageRequest imageCreateModel, CancellationToken cancellationToken = default) =>
        await _httpClient.PostAndReadAsAsync<ImageResponse>(_endpointProvider.ImageCreate(), imageCreateModel, cancellationToken);

    /// <summary>
    ///     Creates an edited or extended image given an original image and a prompt.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ImageResponse> CreateImageEdit(CreateImageEditRequest imageEditCreateRequest, CancellationToken cancellationToken = default)
    {
        var multipartContent = new MultipartFormDataContent();
        if (imageEditCreateRequest.User != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.User), "user");
        }

        if (imageEditCreateRequest.ResponseFormat != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.ResponseFormat), "response_format");
        }

        if (imageEditCreateRequest.Size != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Size), "size");
        }

        if (imageEditCreateRequest.N != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.N.Value.ToString(CultureInfo.InvariantCulture)), "n");
        }

        if (imageEditCreateRequest.Model != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Model), "model");
        }

        if (imageEditCreateRequest.Mask != null)
        {
            multipartContent.Add(new StreamContent(imageEditCreateRequest.Mask.Data), "mask", imageEditCreateRequest.Mask.Name);
        }

        if (imageEditCreateRequest.Background != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Background), "background");
        }

        if (imageEditCreateRequest.OutputFormat != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.OutputFormat), "output_format");
        }

        if (imageEditCreateRequest.OutputCompression != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.OutputCompression.Value.ToString(CultureInfo.InvariantCulture)), "output_compression");
        }

        if (imageEditCreateRequest.Quality != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Quality), "quality");
        }

        multipartContent.Add(new StringContent(imageEditCreateRequest.Prompt), "prompt");
        if (imageEditCreateRequest.Image.AsItem != null)
        {
            multipartContent.Add(new StreamContent(imageEditCreateRequest.Image.AsItem.Data), "image", imageEditCreateRequest.Image.AsItem.Name);
        }
        else if (imageEditCreateRequest.Image.AsList != null)
        {
            foreach (var image in imageEditCreateRequest.Image.AsList)
            {
                multipartContent.Add(new StreamContent(image.Data), "image[]", image.Name);
            }
        }

        return await _httpClient.PostFileAndReadAsAsync<ImageResponse>(_endpointProvider.ImageEditCreate(), multipartContent, cancellationToken);
    }

    /// <summary>
    ///     Creates a variation of a given image.
    /// </summary>
    /// <param name="imageEditCreateRequest"></param>
    /// <returns></returns>
    public async Task<ImageResponse> CreateImageVariation(CreateImageVariationRequest imageEditCreateRequest, CancellationToken cancellationToken = default)
    {
        var multipartContent = new MultipartFormDataContent();
        if (imageEditCreateRequest.User != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.User), "user");
        }

        if (imageEditCreateRequest.ResponseFormat != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.ResponseFormat), "response_format");
        }

        if (imageEditCreateRequest.Size != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Size), "size");
        }

        if (imageEditCreateRequest.N != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.N.Value.ToString(CultureInfo.InvariantCulture)), "n");
        }

        if (imageEditCreateRequest.Model != null)
        {
            multipartContent.Add(new StringContent(imageEditCreateRequest.Model!), "model");
        }

        multipartContent.Add(new StreamContent(imageEditCreateRequest.Image.Data), "image", imageEditCreateRequest.Image.Name);

        return await _httpClient.PostFileAndReadAsAsync<ImageResponse>(_endpointProvider.ImageVariationCreate(), multipartContent, cancellationToken);
    }
}