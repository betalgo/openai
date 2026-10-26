using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class ToolConverterTests
{
    [Fact]
    public void UnknownToolTypesRoundTripAsRawTool()
    {
        const string json = """{"type":"future_tool","name":"demo","config":{"enabled":true}}""";

        var tool = JsonSerializer.Deserialize<ITool>(json);
        var serialized = JsonSerializer.Serialize(tool);

        tool.ShouldBeOfType<RawTool>();
        tool.Type.ShouldBe("future_tool");
        serialized.ShouldBe(json);
    }
}
