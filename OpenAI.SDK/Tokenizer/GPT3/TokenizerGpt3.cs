// Inspired from @author: Devis Lucato.

using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;

namespace OpenAI.Tokenizer.GPT3;

/// <summary>
///     GPT3 Tokenizer.
/// </summary>
public static class TokenizerGpt3
{
    private static readonly ConcurrentDictionary<string, string> BpeCache = new();
    private static readonly ConcurrentDictionary<int, char> BytesToUnicodeCache = InitializeBytesToUnicodeCache();
    private static readonly Regex EncodingRegex = new(@"'s|'t|'re|'ve|'m|'ll|'d| ?\p{L}+| ?\p{N}+| ?[^\s\p{L}\p{N}]+|\s+(?!\S)|\s+", RegexOptions.Compiled);

    /// <summary>
    ///     Encode This method use LF style EOL, if you use CR LF style EOL you need to set cleanUpWindowsEOL to true
    /// </summary>
    /// <param name="text"></param>
    /// <param name="cleanUpCREOL">set it true to get rid of CR</param>
    /// <returns></returns>
    public static IEnumerable<int> Encode(string text, bool cleanUpCREOL = false)
    {
        if (string.IsNullOrEmpty(text))
        {
            yield break;
        }

        if (cleanUpCREOL)
        {
            text = text.Replace("\r", "");
        }

        foreach (var newToken in GetNewTokens(text))
        {
            yield return TokenizerGpt3Settings.Encoder[newToken];
        }
    }

    /// <summary>
    ///     Get token count. This method use LF style EOL, if you use CR LF style EOL you need to set cleanUpWindowsEOL to true
    /// </summary>
    /// <param name="text"></param>
    /// <param name="cleanUpCREOL">set it true to get rid of CR</param>
    /// <returns></returns>
    public static int TokenCount(string text, bool cleanUpCREOL = false)
    {
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        if (cleanUpCREOL)
        {
            text = text.Replace("\r", "");
        }

        return GetNewTokens(text).Count();
    }

    private static IEnumerable<string> GetNewTokens(string text)
    {
        var byteEncoder = BytesToUnicodeCache;
        var matches = EncodingRegex.Matches(text);

        foreach (var match in matches.Cast<Match>())
        {
            var tokenBytes = Encoding.UTF8.GetBytes(match.Value);
            var token = new string(Array.ConvertAll(tokenBytes, x => byteEncoder[x]));
            var newTokens = BytePairEncoding(token).Split(' ');

            foreach (var newToken in newTokens)
            {
                yield return newToken;
            }
        }
    }


    private static int Ord(string x)
    {
        return char.ConvertToUtf32(x, 0);
    }

    private static ConcurrentDictionary<int, char> InitializeBytesToUnicodeCache()
    {
        var bytes = Enumerable.Range(Ord("!"), Ord("~") + 1 - Ord("!"))
            .Concat(Enumerable.Range(Ord("¡"), Ord("¬") + 1 - Ord("¡")))
            .Concat(Enumerable.Range(Ord("®"), Ord("ÿ") + 1 - Ord("®")))
            .ToList();

        var chars = (from x in bytes select (char) x).ToList();

        var n = 0;
        for (var b = 0; b < 256; b++)
        {
            if (bytes.Contains(b))
            {
                continue;
            }

            bytes.Add(b);
            chars.Add((char) (256 + n++));
        }

        return new ConcurrentDictionary<int, char>(bytes
            .Zip(chars, (k, v) => new {k, v})
            .ToDictionary(x => x.k, x => x.v));
    }

    private static string BytePairEncoding(string token)
    {
        if (BpeCache.TryGetValue(token, out var cachedResult))
        {
            return cachedResult;
        }

        List<string> word = (from x in token.ToList() select x.ToString()).ToList();
        var pairs = GetPairs(word);
        if (pairs.Count == 0)
        {
            BpeCache.TryAdd(token, token);
            return token;
        }

        while (true)
        {
            var minPairs = new SortedDictionary<long, Tuple<string, string>>();

            foreach (var pair in pairs.SelectMany(pair => pair.Value.Select(pairValue => new Tuple<string, string>(pair.Key, pairValue))))
            {
                if (TokenizerGpt3Settings.BpeRanks.TryGetValue(pair, out var rank))
                {
                    minPairs[rank] = pair;
                }
                else
                {
                    minPairs[100000000000] = pair;
                }
            }

            var biGram = minPairs[minPairs.Keys.Min()];
            if (!TokenizerGpt3Settings.BpeRanks.ContainsKey(biGram))
            {
                break;
            }

            var first = biGram.Item1;
            var second = biGram.Item2;

            var newWord = new List<string>();
            var i = 0;

            while (i < word.Count)
            {
                var j = word.IndexOf(first, i);

                if (j == -1)
                {
                    var slice = new ArraySegment<string>((from x in word select x.ToString()).ToArray(), i, word.Count - i);
                    newWord.AddRange(slice);
                    break;
                }

                var slice2 = new ArraySegment<string>((from x in word select x.ToString()).ToArray(), i, j - i);
                newWord.AddRange(slice2);
                i = j;

                if (word[i] == first && i < word.Count - 1 && word[i + 1] == second)
                {
                    newWord.Add($"{first}{second}");
                    i += 2;
                }
                else
                {
                    newWord.Add(word[i]);
                    i += 1;
                }
            }

            word = newWord;
            if (word.Count == 1)
            {
                break;
            }

            pairs = GetPairs(word);
        }

        var result = string.Join(" ", word);
        BpeCache.TryAdd(token, result);
        return result;
    }

    private static Dictionary<string, List<string>> GetPairs(IReadOnlyList<string> word)
    {
        var result = new Dictionary<string, List<string>>();

        var prevChar = word[0];
        for (var i = 1; i < word.Count; i++)
        {
            var currentChar = word[i];
            if (!result.ContainsKey(prevChar))
            {
                result[prevChar] = new List<string>();
            }

            result[prevChar].Add(currentChar);
            prevChar = currentChar;
        }

        return result;
    }
}