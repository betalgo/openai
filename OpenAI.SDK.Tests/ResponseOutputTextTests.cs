using System.Text.Json;
using Betalgo.Ranul.OpenAI.Contracts.Responses.Responses;
using Betalgo.Ranul.OpenAI.Contracts.Types;
using Betalgo.Ranul.OpenAI.Contracts.Types.Content;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;

namespace OpenAI.SDK.Tests;

public class ResponseOutputTextTests
{
    [Fact]
    public void DeserializedResponseAggregatesOnlyTextInOutputOrder()
    {
        const string json = """
            {"id":"resp_1","output":[
              {"type":"function_call","id":"fc_1","call_id":"call_1","name":"lookup","arguments":"{}"},
              {"type":"message","id":"msg_1","status":"completed","content":[
                {"type":"output_text","text":"Hello","annotations":[]},
                {"type":"refusal","refusal":"excluded"},
                {"type":"output_text","text":" ","annotations":[]}]},
              {"type":"message","id":"msg_2","status":"completed","content":[
                {"type":"output_text","text":"world!","annotations":[]}]}
            ]}
            """;

        var response = JsonSerializer.Deserialize<Response>(json)!;

        Assert.Equal("Hello world!", response.OutputText);
        var roundTrip = JsonSerializer.Deserialize<Response>(JsonSerializer.Serialize(response))!;
        Assert.Equal("Hello world!", roundTrip.OutputText);
        Assert.Equal(3, roundTrip.Output!.Count);
    }

    [Theory]
    [InlineData("{}")]
    [InlineData("{\"output\":[]}")]
    [InlineData("{\"output\":[{\"type\":\"message\",\"content\":[{\"type\":\"refusal\",\"refusal\":\"No\"}]}]}")]
    public void ResponseWithoutTextReturnsEmptyString(string json)
    {
        Assert.Equal(string.Empty, JsonSerializer.Deserialize<Response>(json)!.OutputText);
    }

    [Fact]
    public void AggregationReflectsChangesToOutput()
    {
        var text = new OutputTextContent("First");
        var response = new Response
        {
            Output = new List<IOutputItem> { new OutputMessageItem { Content = new List<IContent> { text } } }
        };
        Assert.Equal("First", response.OutputText);
        text.Text = "Updated";
        Assert.Equal("Updated", response.OutputText);
    }

    [Fact]
    public void ExplicitOutputTextIsPreservedAndCanBeReset()
    {
        var response = JsonSerializer.Deserialize<Response>("""{"output_text":"explicit","output":[]}""")!;
        Assert.Equal("explicit", response.OutputText);
        Assert.Equal("explicit", JsonSerializer.Deserialize<Response>(JsonSerializer.Serialize(response))!.OutputText);
        response.OutputText = "";
        Assert.Equal("", response.OutputText);
        response.OutputText = null;
        Assert.Equal("", response.OutputText);
    }
}
