// Inspired from @author: Devis Lucato.

using System.Globalization;
using System.Reflection;

namespace OpenAI.GPT3.Tokenizer.TikToken;

/// <summary>
///  load embedded resource
/// </summary>
internal static class EmbeddedResource
{
    private static readonly string? Namespace = typeof(EmbeddedResource).Namespace;

    /// <summary>
    ///  Read a resource file
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    internal static string Read(string name)
    {
        var assembly = typeof(EmbeddedResource).GetTypeInfo().Assembly;
        if (assembly == null)
        {
            throw new NullReferenceException($"[{Namespace}] {name} assembly not found");
        }

        using var resource = assembly.GetManifestResourceStream($"{Namespace}." + name);
        if (resource == null)
        {
            throw new NullReferenceException($"[{Namespace}] {name} resource not found");
        }

        using var reader = new StreamReader(resource);
        return reader.ReadToEnd();
    }


    /// <summary>
    ///  Load a resource file, split into lines, split every line by space, and return a dictionary of byte[] to int
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    internal static Dictionary<byte[], int> LoadRanks(string name)
    {
        string contents = Read(name);
        var lines = contents.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        var ranks = lines
            .Where(line => !string.IsNullOrEmpty(line))
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

        return ranks.ToDictionary(
            strs => Convert.FromBase64String(strs[0]),
            strs => int.Parse(strs[1], CultureInfo.InvariantCulture)
        );
    }
}