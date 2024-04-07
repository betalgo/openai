using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Running;
using OpenAI.Tokenizer.GPT3;

namespace OpenAI.BenchmarkTests;

public class TokenizerGpt3Benchmark
{
    private string _sampleText = null!;

    [GlobalSetup]
    public void Setup()
    {
        // Load sample text from a file or generate it programmatically
        _sampleText = File.ReadAllText("Sample-text.txt");
    }

    [Benchmark]
    public void EncodeOriginal()
    {
        _ = TokenizerGpt3.Encode(_sampleText).ToList();
    }

    [Benchmark]
    public void EncodeOptimized()
    {
        // Replace this with your optimized Encode method
        _ = TokenizerGpt3Optimized.Encode(_sampleText).ToList();
    }

    [Benchmark]
    public void TokenCountOriginal()
    {
        _ = TokenizerGpt3.TokenCount(_sampleText);
    }

    [Config(typeof(Config))]
    private class Config : ManualConfig
    {
        public Config()
        {
            AddDiagnoser(MemoryDiagnoser.Default);
            AddExporter(CsvExporter.Default);
            AddExporter(MarkdownExporter.GitHub);
            AddExporter(HtmlExporter.Default);
        }
    }

    [Benchmark]
    public void TokenCountOptimized()
    {
        // Replace this with your optimized TokenCount method
        _ = TokenizerGpt3Optimized.TokenCount(_sampleText);
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<TokenizerGpt3Benchmark>();
    }
}