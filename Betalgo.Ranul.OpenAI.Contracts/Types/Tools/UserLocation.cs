using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     User location information for web search.
///     <see href="https://platform.openai.com/docs/guides/tools-web-search">Web Search Guide</see>.
/// </summary>
public class UserLocation
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserLocation" /> class.
    /// </summary>
    public UserLocation()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserLocation" /> class with parameters.
    /// </summary>
    /// <param name="city">The city name.</param>
    /// <param name="country">The country name.</param>
    /// <param name="region">The region or state name.</param>
    /// <param name="timezone">The timezone.</param>
    public UserLocation(string? city = null, string? country = null, string? region = null, string? timezone = null)
    {
        City = city;
        Country = country;
        Region = region;
        Timezone = timezone;
    }

    /// <summary>
    ///     The type of location. Always <c>approximate</c>.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "approximate";

    /// <summary>
    ///     The city name.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    /// <summary>
    ///     The country name.
    /// </summary>
    [JsonPropertyName("country")]
    public string? Country { get; set; }

    /// <summary>
    ///     The region or state name.
    /// </summary>
    [JsonPropertyName("region")]
    public string? Region { get; set; }

    /// <summary>
    ///     The timezone.
    /// </summary>
    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }
}

