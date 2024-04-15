using System.Text.Json;

namespace OpenAI.Playground;

public static class StringExtension
{
    public static string? ToJson(this object? s)
    {
        return s == null ? null : JsonSerializer.Serialize(s);
    }

    public static T? D<T>(this string json) where T : class
    {
        return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<T>(json);
    }
}