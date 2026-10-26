using System.Reflection;
using System.Runtime.Versioning;
using Betalgo.Ranul.OpenAI.Extensions.Streaming;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class TargetSelectionTests
{
    [Fact]
    public void TestsExecuteTheNetStandardAssembly()
    {
        typeof(StreamParser).Assembly.GetCustomAttribute<TargetFrameworkAttribute>()!
            .FrameworkName.ShouldBe(".NETStandard,Version=v2.0");
    }
}
