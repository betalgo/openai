using System;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace OpenAI.GPT3.Tokenizer.TikToken;

/// <summary>
///  Byte Pair Encoder Core, porting from [Tiktoken/lib](https://github.com/openai/tiktoken/blob/main/src/lib.rs) which is written in Rust
/// </summary>
class CoreBPE
{
    /// <summary>
    ///  The encoder dictionary
    /// </summary>
    private Dictionary<byte[], int> Encoder { get; }
    /// <summary>
    ///  The special tokens encoder dictionary
    /// </summary>
    private Dictionary<string, int> SpecialTokensEncoder { get; }
    /// <summary>
    ///  The decoder dictionary
    /// </summary>
    private Dictionary<int, byte[]> Decoder { get; }
    /// <summary>
    ///  The special tokens decoder dictionary
    /// </summary>
    private Dictionary<int, byte[]> SpecialTokensDecoder { get; }
    /// <summary>
    ///  The regex
    /// </summary>
    private Regex Regex { get; }
    /// <summary>
    ///  The special regex
    /// </summary>
    private Regex SpecialRegex { get; }

    /// <summary>
    ///  constructor
    /// </summary>
    /// <param name="encoder"></param>
    /// <param name="specialTokensEncoder"></param>
    /// <param name="pattern"></param>
    /// <exception cref="Exception"></exception>
    public CoreBPE(
        Dictionary<byte[], int> encoder,
        Dictionary<string, int> specialTokensEncoder,
        string pattern)
    {
        try
        {
            var regex = new Regex(pattern);

            var specialRegex = new Regex(string.Join("|", specialTokensEncoder.Keys.Select(s => Regex.Escape(s))));

            var decoder = encoder.ToDictionary(kv => kv.Value, kv => kv.Key);

            Debug.Assert(encoder.Count == decoder.Count);

            var specialTokensDecoder = specialTokensEncoder.ToDictionary(kv => kv.Value, kv => kv.Key.Select(c => (byte)c).ToArray());

            // comparer is necessary, otherwise the byte[] comparison will fail
            var comparer = new ByteArrayEqualityComparer();
            this.Encoder = encoder == null
                ? new Dictionary<byte[], int>(comparer)
                : new Dictionary<byte[], int>(encoder, comparer);
            this.SpecialTokensEncoder = specialTokensEncoder;
            this.Decoder = decoder;
            this.SpecialTokensDecoder = specialTokensDecoder;
            this.Regex = regex;
            this.SpecialRegex = specialRegex;
        }
        catch (ArgumentException e)
        {
            throw new Exception(e.Message);
        }
    }


    /// <summary>
    /// BytePairMerge
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="piece"></param>
    /// <param name="ranks"></param>
    /// <param name="f"></param>
    /// <returns></returns>

    // Basic principles:
    // 1. Combine two adjacent bytes of the piece and look them up in the dictionary.
    // 2. For successful matches in step 1, try to take one more byte forward and backward and compare with the dictionary.
    // 3. Repeat the above steps to break the piece into multiple tokens according to the dictionary, with each token being as long as possible.
    // Note: due to the lack of a direct equivalent to Rust's FxHashMap data structure in C#, a regular Dictionary<byte[], int> was used, which may result in performance differences.
    //基本原理：
    //1. 将piece相邻的两个字节组合起来，在字典中查找
    //2. 对步骤1中匹配成功的记录，尝试向前后多取一个字节，与字典对比
    //3. 重复上述步骤，可以将piece按字典拆解为多个Token，每个Token尽量长(即贪婪原则)
    //注意，由于 C# 中没有直接等价于 Rust 中的 `FxHashMap` 的数据结构，使用了普通的 `Dictionary<byte[], int>`，这可能会导致性能差异。
    public static List<T> BytePairMerge<T>(byte[] piece, Dictionary<byte[], int> ranks, Func<System.Range, T> f)
    {
        List<(int start, int rank)> parts = Enumerable.Range(0, piece.Length + 1)
            .Select(i => (i, int.MaxValue)).ToList();

        Func<List<(int start, int rank)>, int, int, int?> getRank = (parts, startIdx, skip) =>
        {
            if (startIdx + skip + 2 < parts.Count)
            {
                // 下一句代码的作用是： 从 `piece` 中取出 `parts[startIdx].start` 到 `parts[startIdx + skip + 2].start` 之间的一段数据，作为 `key`。
                byte[] key = piece[parts[startIdx].start..parts[startIdx + skip + 2].start];
                if (ranks.TryGetValue(key, out int rank))
                {
                    return rank;
                }
            }
            return null;
        };

        for (int i = 0; i < parts.Count - 2; i++)
        {
            int? rank = getRank(parts, i, 0);
            if (rank.HasValue)
            {
                parts[i] = (parts[i].start, rank.Value);
            }
        }

        while (parts.Count > 1)
        {
            (int minRank, int minIdx) = (int.MaxValue, 0);
            for (int i = 0; i < parts.Count - 1; i++)
            {
                (int _, int rank) = parts[i];
                if (rank < minRank)
                {
                    minRank = rank;
                    minIdx = i;
                }
            }
            if (minRank != int.MaxValue)
            {
                int i = minIdx;

                parts[i] = (parts[i].start, getRank(parts, i, 1) ?? int.MaxValue);
                if (i > 0)
                {
                    parts[i - 1] = (parts[i - 1].start, getRank(parts, i - 1, 1) ?? int.MaxValue);
                }

                parts.RemoveAt(i + 1);
            }
            else
            {
                break;
            }
        }

        List<T> output = new List<T>(parts.Count - 1);
        for (int i = 0; i < parts.Count - 1; i++)
        {
            output.Add(f(parts[i].start..parts[i + 1].start));
        }

        return output;
    }

    /// <summary>
    ///  BytePairEncode
    /// </summary>
    /// <param name="piece"></param>
    /// <param name="ranks"></param>
    /// <returns></returns>
    public static List<int> BytePairEncode(byte[] piece, Dictionary<byte[], int> ranks)
    {
        if (piece.Length == 1)
        {
            return new List<int> { ranks[piece] };
        }
        return BytePairMerge(piece, ranks, p => ranks[piece[p.Start..p.End]]);
    }

    /// <summary>
    ///  Encode text into tokens
    /// </summary>
    /// <param name="text"></param>
    /// <param name="allowedSpecial"></param>
    /// <returns></returns>
    /// 
    // Basic principles:
    // 1. Use special characters to split the original text into several sections.
    // 2. Use regular expressions to split each section into several smaller segments (or words) based on delimiters (punctuation, spaces, line breaks, etc.).
    // 3. For each segment, first try to look up the encoding dictionary directly; if successful, the encoding is done.
    // 4. If not successful, use BytePairEncode to split the segment into several byte pairs.
    // 5. For special characters at the end of each section, encode them directly.
    //基本原理：
    //1. 使用特殊字符将原文本分割为若干段
    //2. 对每段文本，使用正则匹配，将原文本按照分隔符(标点、空格、分行等)分为若干小段(或单词)；
    //3. 对每一小段，先尝试直接查询编码表，成功的话编码结束；
    //4. 如果没有成功，那么就使用 `BytePairEncode` ，按照贪婪原则将这一小段分割为若干个字节对，逐个编码
    //5. 对于每段文本末尾的特殊字符，直接编码
    public (int[], int) EncodeNative(string text, HashSet<string> allowedSpecial)
    {
        List<int> ret = new List<int>();

        int start = 0;
        int lastPieceTokenLen = 0;
        while (true)
        {
            Match nextSpecial;
            int startFind = start;
            while (true)
            {
                nextSpecial = this.SpecialRegex.Match(text, startFind);
                if (nextSpecial.Success)
                {
                    string value = text.Substring(nextSpecial.Index, nextSpecial.Length);
                    if (allowedSpecial.Contains(value))
                    {
                        break;
                    }
                    startFind = nextSpecial.Index + 1;
                }
                else
                {
                    break;
                }
            }
            int end = nextSpecial.Success ? nextSpecial.Index : text.Length;

            foreach (Match match in this.Regex.Matches(text.Substring(start, end - start)))
            {
                byte[] piece = System.Text.Encoding.UTF8.GetBytes(match.Value);
                if (this.Encoder.TryGetValue(piece, out int token))
                {
                    lastPieceTokenLen = 1;
                    ret.Add(token);
                    continue;
                }
                List<int> tokens = BytePairEncode(piece, this.Encoder);
                lastPieceTokenLen = tokens.Count;
                ret.AddRange(tokens);
            }

            if (nextSpecial.Success)
            {
                string piece = text.Substring(nextSpecial.Index, nextSpecial.Length);
                int token = this.SpecialTokensEncoder[piece];
                ret.Add(token);
                start = nextSpecial.Index + nextSpecial.Length;
                lastPieceTokenLen = 0;
            }
            else
            {
                break;
            }
        }

        return (ret.ToArray(), lastPieceTokenLen);
    }

    /// <summary>
    ///  Encode text into tokens
    /// </summary>
    /// <param name="text"></param>
    /// <param name="allowedSpecial"></param>
    /// <returns></returns>
    public int[] Encode(string text, HashSet<string> allowedSpecial)
    {
        return this.EncodeNative(text, allowedSpecial).Item1;
    }

    /// <summary>
    ///  Decode tokens into text
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    public byte[] DecodeNative(int[] tokens)
    {
        List<byte> ret = new List<byte>(tokens.Length * 2);
        foreach (int token in tokens)
        {
            byte[] tokenBytes = this.Decoder.TryGetValue(token, out byte[]? value)
                ? value
                : this.SpecialTokensDecoder[token];
            ret.AddRange(tokenBytes);
        }
        return ret.ToArray();
    }

    /// <summary>
    /// Decode tokens into text
    /// </summary>
    /// <param name="tokens"></param>
    /// <returns></returns>
    public string Decode(int[] tokens)
    {
        var bytes = this.DecodeNative(tokens);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    ///  byte[] comparer
    /// </summary>
    internal sealed class ByteArrayEqualityComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[]? x, byte[]? y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return ReferenceEquals(x, y) || StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
        }

        public int GetHashCode(byte[] obj)
        {
            return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
        }
    }
}





