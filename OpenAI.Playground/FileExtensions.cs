namespace OpenAI.Playground;

using System.IO;
using System.Threading.Tasks;

public static class FileExtensions
{
    public static async Task<byte[]> ReadAllBytesAsync(string path)
    {
#if NET6_0_OR_GREATER
        return await File.ReadAllBytesAsync(path);
#else
        byte[] buffer;
        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
        {
            buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, (int)stream.Length);
        }
        return buffer;
#endif
    }

    public static async Task<string> ReadAllTextAsync(string path)
    {
#if NET6_0_OR_GREATER
        return await File.ReadAllTextAsync(path);
#else
        string text;
        using (var reader = new StreamReader(path))
        {
            text = await reader.ReadToEndAsync();
        }
        return text;
#endif
    }
}
