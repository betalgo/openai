namespace OpenAI.GPT3.ObjectModels.RequestModels;

public record ImageEditCreateRequest : ImageCreateRequest
{
    /// <summary>
    ///     The image to edit. Must be a valid PNG file, less than 4MB, and square.
    /// </summary>
    public byte[] Image { get; set; }

    /// <summary>
    ///     Image file name
    /// </summary>
    public string ImageName { get; set; }

    /// <summary>
    ///     An additional image whose fully transparent areas (e.g. where alpha is zero) indicate where image should be edited.
    ///     Must be a valid PNG file, less than 4MB, and have the same dimensions as image.
    /// </summary>
    public byte[]? Mask { get; set; }

    /// <summary>
    ///     Mask file name
    /// </summary>
    public string? MaskName { get; set; }
}