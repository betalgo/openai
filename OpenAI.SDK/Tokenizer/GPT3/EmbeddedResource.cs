// Inspired from @author: Devis Lucato.

using System.Reflection;

namespace OpenAI.Tokenizer.GPT3;

internal static class EmbeddedResource
{
    private static readonly string? Namespace = typeof(EmbeddedResource).Namespace;

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
}