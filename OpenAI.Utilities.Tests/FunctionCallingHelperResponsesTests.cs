using System.Text.Json;
using Betalgo.OpenAI.Utilities.FunctionCalling;
using Betalgo.Ranul.OpenAI.Contracts.Types.Items;

namespace OpenAI.Utilities.Tests;

public class FunctionCallingHelperResponsesTests
{
    [Fact]
    public void CallFunctionExecutesResponsesFunctionToolCalls()
    {
        var obj = new FunctionCallingTestClass();
        var functionToolCall = new FunctionToolCallItem(
            "call_123",
            "TestFunction",
            "{\"intParameter\": 1, \"floatParameter\": 2.0, \"boolParameter\": true, \"stringParameter\": \"Hello\", \"enumParameter\": \"Value1\", \"enumParameter2\": \"Value2\", \"requiredIntParameter\": 1, \"notRequiredIntParameter\": 2, \"OverriddenName\": 3}");

        var result = FunctionCallingHelper.CallFunction<int>(functionToolCall, obj);

        result.ShouldBe(5);
        obj.IntParameter.ShouldBe(1);
        obj.FloatParameter.ShouldBe(2.0f);
        obj.BoolParameter.ShouldBe(true);
        obj.StringParameter.ShouldBe("Hello");
        obj.EnumParameter.ShouldBe(TestEnum.Value1);
        obj.EnumParameter2.ShouldBe(TestEnum.Value2);
        obj.RequiredIntParameter.ShouldBe(1);
        obj.NotRequiredIntParameter.ShouldBe(2);
        obj.OverriddenNameParameter.ShouldBe(3);
    }

    [Fact]
    public void CreateFunctionCallOutputPassesThroughStrings()
    {
        var functionToolCall = new FunctionToolCallItem("call_123", "SecondFunction", "{}");

        var output = FunctionCallingHelper.CreateFunctionCallOutput(functionToolCall, "Hello");

        output.CallId.ShouldBe("call_123");
        output.Output.ShouldBe("Hello");
    }

    [Fact]
    public void CreateFunctionCallOutputSerializesNonStringResults()
    {
        var functionToolCall = new FunctionToolCallItem("call_123", "SecondFunction", "{}");

        var output = FunctionCallingHelper.CreateFunctionCallOutput(functionToolCall, new { value = 42 });

        output.CallId.ShouldBe("call_123");
        output.Output.ShouldBeOfType<string>();

        using var json = JsonDocument.Parse((string)output.Output);
        json.RootElement.GetProperty("value").GetInt32().ShouldBe(42);
    }
}
