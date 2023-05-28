// Inspired from @author: Devis Lucato.

using System.Text.Json;

namespace OpenAI.Tokenizer.GPT3;

/// <summary>
///     GPT3 Settings.
/// </summary>
internal static class TokenizerGpt3Settings
{
    private static readonly Lazy<Dictionary<string, int>> EncoderLazy = new(BuildEncoder);
    private static readonly Lazy<Dictionary<Tuple<string, string>, int>> BpeRanksLazy = new(BuildBpeRanks);
    private static readonly string? Namespace = typeof(TokenizerGpt3Settings).Namespace;
    internal static Dictionary<string, int> Encoder => EncoderLazy.Value;
    internal static Dictionary<Tuple<string, string>, int> BpeRanks => BpeRanksLazy.Value;

    private static Dictionary<Tuple<string, string>, int> BuildBpeRanks()
    {
        var lines = EmbeddedResource.Read("vocab.bpe").Split('\n');
        var bpeMerges = new ArraySegment<string>(lines, 1, lines.Length - 1)
            .Where(x => x.Trim().Length > 0)
            .Select(x =>
            {
                var y = x.Split(' ');
                return new Tuple<string, string>(y[0], y[1]);
            }).ToList();
        return DictZip(bpeMerges, Range(0, bpeMerges.Count));
    }

    private static Dictionary<string, int> BuildEncoder()
    {
        var json = EmbeddedResource.Read("encoder.json");
        var encoder = JsonSerializer.Deserialize<Dictionary<string, int>>(json, new JsonSerializerOptions());
        if (encoder == null)
        {
            throw new NullReferenceException($"[{Namespace}] encoder.json deserialization returned NULL");
        }

        return encoder;
    }

    private static Dictionary<Tuple<string, string>, int> DictZip(IReadOnlyList<Tuple<string, string>> x, IReadOnlyList<int> y)
    {
        var result = new Dictionary<Tuple<string, string>, int>();
        for (var i = 0; i < x.Count; i++)
        {
            result.Add(x[i], y[i]);
        }

        return result;
    }

    private static List<int> Range(int x, int y)
    {
        return Enumerable.Range(x, y - x).ToList();
    }
}