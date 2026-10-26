using System.Text.Json.Serialization;

namespace Betalgo.Ranul.OpenAI.Contracts.Types.Tools;

/// <summary>
///     Ranking options for file search.
///     <see href="https://platform.openai.com/docs/guides/tools-file-search">File Search Guide</see>.
/// </summary>
public class RankingOptions
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="RankingOptions" /> class.
    /// </summary>
    public RankingOptions()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="RankingOptions" /> class with parameters.
    /// </summary>
    /// <param name="ranker">The ranker to use.</param>
    /// <param name="scoreThreshold">The minimum score threshold.</param>
    public RankingOptions(string? ranker = null, double? scoreThreshold = null)
    {
        Ranker = ranker;
        ScoreThreshold = scoreThreshold;
    }

    /// <summary>
    ///     The ranker to use.
    /// </summary>
    [JsonPropertyName("ranker")]
    public string? Ranker { get; set; }

    /// <summary>
    ///     The minimum score threshold.
    /// </summary>
    [JsonPropertyName("score_threshold")]
    public double? ScoreThreshold { get; set; }
}

