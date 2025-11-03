using Betalgo.Ranul.OpenAI.Contracts.Enums.Image;
using Betalgo.Ranul.OpenAI.Contracts.Enums.Model;

namespace Betalgo.Ranul.OpenAI.Contracts.Interfaces;

public interface IHasImageBackground
{
    ImageBackground? Background { get; set; }
}

public interface IHasImageOutputFormat
{
    ImageOutputFormat? OutputFormat { get; set; }
}

public interface IHasImageQuality
{
    ImageQuality? Quality { get; set; }
}

public interface IHasImageSize
{
    ImageSize? Size { get; set; }
}

public interface IHasModelImage
{
    ImageModel? Model { get; set; }
}