using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Types.Tools;
using Shouldly;

namespace OpenAI.SDK.Tests;

public class CodeInterpreterContainerTests
{
    [Theory]
    [InlineData("\"cntr_existing\"")]
    [InlineData("{\"type\":\"auto\"}")]
    public void ContainerIdAndAutomaticConfigurationRoundTrip(string container)
    {
        var json = "{\"type\":\"code_interpreter\",\"container\":" + container + "}";
        var tool = JsonSerializer.Deserialize<CodeInterpreterTool>(json)!;
        using var result = JsonDocument.Parse(JsonSerializer.Serialize(tool));
        result.RootElement.GetProperty("container").GetRawText().ShouldBe(container);
    }
}
