using OpenAI.ObjectModels.RequestModels;
using OpenAI.Utilities.FunctionCalling;

namespace OpenAI.Utilities.Tests;

public class FunctionCallingHelperTests
{
    [Fact]
    public void VerifyGetFunctionDefinition()
    {
        var functionDefinition = FunctionCallingHelper.GetFunctionDefinition(typeof(FunctionCallingTestClass).GetMethod("TestFunction")!);

        functionDefinition.Name.ShouldBe("TestFunction");
        functionDefinition.Description.ShouldBe("Test Function");
        functionDefinition.Parameters.ShouldNotBeNull();
        functionDefinition.Parameters.Properties!.Count.ShouldBe(9);

        var intParameter = functionDefinition.Parameters.Properties["intParameter"];
        intParameter.Description.ShouldBe("Int Parameter");
        intParameter.Type.ShouldBe("integer");

        var floatParameter = functionDefinition.Parameters.Properties["floatParameter"];
        floatParameter.Description.ShouldBe("Float Parameter");
        floatParameter.Type.ShouldBe("number");

        var boolParameter = functionDefinition.Parameters.Properties["boolParameter"];
        boolParameter.Description.ShouldBe("Bool Parameter");
        boolParameter.Type.ShouldBe("boolean");

        var stringParameter = functionDefinition.Parameters.Properties["stringParameter"];
        stringParameter.Description.ShouldBe("String Parameter");
        stringParameter.Type.ShouldBe("string");

        var enumValues = new List<string> {"Value1", "Value2", "Value3"};

        var enumParameter = functionDefinition.Parameters.Properties["enumParameter"];
        enumParameter.Description.ShouldBe("Enum Parameter");
        enumParameter.Type.ShouldBe("string");
        enumParameter.Enum.ShouldBe(enumValues);


        var enumParameter2 = functionDefinition.Parameters.Properties["enumParameter2"];
        enumParameter2.Description.ShouldBe("Enum Parameter 2");
        enumParameter2.Type.ShouldBe("string");
        enumParameter2.Enum.ShouldBe(enumValues);

        functionDefinition.Parameters.Properties.ShouldNotContainKey("overriddenNameParameter");
        functionDefinition.Parameters.Properties.ShouldContainKey("OverriddenName");
    }

    [Fact]
    public void VerifyTypeOverride()
    {
        var functionDefinition = FunctionCallingHelper.GetFunctionDefinition(typeof(FunctionCallingTestClass).GetMethod("ThirdFunction")!);

        var overriddenNameParameter = functionDefinition.Parameters.Properties["overriddenTypeParameter"];
        overriddenNameParameter.Type.ShouldBe("string");
        overriddenNameParameter.Description.ShouldBe("Overridden type parameter");
    }

    [Fact]
    public void VerifyGetFunctionDefinitions()
    {
        var functionDefinitions = FunctionCallingHelper.GetFunctionDefinitions<FunctionCallingTestClass>();

        functionDefinitions.Count.ShouldBe(3);

        var functionDefinition = functionDefinitions.First(x => x.Name == "TestFunction");
        functionDefinition.Description.ShouldBe("Test Function");
        functionDefinition.Parameters.ShouldNotBeNull();
        functionDefinition.Parameters.Properties!.Count.ShouldBe(9);

        var functionDefinition2 = functionDefinitions.First(x => x.Name == "SecondFunction");
        functionDefinition2.Description.ShouldBe("Second Function");
        functionDefinition2.Parameters.ShouldNotBeNull();
        functionDefinition2.Parameters.Properties!.Count.ShouldBe(0);

        var functionDefinition3 = functionDefinitions.First(x => x.Name == "ThirdFunction");
        functionDefinition3.Description.ShouldBe("Third Function");
        functionDefinition3.Parameters.ShouldNotBeNull();
        functionDefinition3.Parameters.Properties!.Count.ShouldBe(1);
    }

    [Fact]
    public void VerifyCallFunction_Simple()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "SecondFunction"
        };

        var result = FunctionCallingHelper.CallFunction<string>(functionCall, obj);
        result.ShouldBe("Hello");
    }

    [Fact]
    public void VerifyCallFunction_Complex()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "TestFunction",
            // arguments is a json dictionary
            Arguments =
                "{\"intParameter\": 1, \"floatParameter\": 2.0, \"boolParameter\": true, \"stringParameter\": \"Hello\", \"enumParameter\": \"Value1\", \"enumParameter2\": \"Value2\", \"requiredIntParameter\": 1, \"notRequiredIntParameter\": 2, \"OverriddenName\": 3}"
        };

        var result = FunctionCallingHelper.CallFunction<int>(functionCall, obj);
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
    public void VerifyCallFunction_ArgumentsDoNotMatch()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "TestFunction",
            Arguments = "{\"intParameter\": \"invalid\", \"floatParameter\": true, \"boolParameter\": 1, \"stringParameter\": 123, \"enumParameter\": \"NonExistentValue\"}"
        };

        Should.Throw<Exception>(() => FunctionCallingHelper.CallFunction<int>(functionCall, obj));
    }

    [Fact]
    public void CallFunctionShouldThrowIfObjIsNull()
    {
        var functionCall = new FunctionCall
        {
            Name = "SecondFunction"
        };

        Should.Throw<ArgumentNullException>(() => FunctionCallingHelper.CallFunction<string>(functionCall, null!));
    }

    [Fact]
    public void CallFunctionShouldThrowIfFunctionCallIsNull()
    {
        var obj = new FunctionCallingTestClass();

        Should.Throw<ArgumentNullException>(() => FunctionCallingHelper.CallFunction<string>(null!, obj));
    }

    [Fact]
    public void CallFunctionShouldThrowIfFunctionCallNameIsNotSet()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = null!
        };

        Should.Throw<InvalidFunctionCallException>(() => FunctionCallingHelper.CallFunction<string>(functionCall, obj));
    }

    [Fact]
    public void CallFunctionShouldThrowIfFunctionCallNameIsNotValid()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "NonExistentFunction"
        };

        Should.Throw<InvalidFunctionCallException>(() => FunctionCallingHelper.CallFunction<string>(functionCall, obj));
    }

    [Fact]
    public void CallFunctionShouldThrowIfInvalidReturnType()
    {
        var obj = new FunctionCallingTestClass();
        var functionCall = new FunctionCall
        {
            Name = "SecondFunction"
        };

        Should.Throw<InvalidFunctionCallException>(() => FunctionCallingHelper.CallFunction<int>(functionCall, obj));
    }

    [Fact]
    public void VerifyCallFunctionWithOverriddenType()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "ThirdFunction",
            Arguments = "{\"overriddenTypeParameter\": 1}"
        };

        FunctionCallingHelper.CallFunction<object>(functionCall, obj);
        obj.OverriddenTypeParameter.ShouldBe("1");
    }
}

internal class FunctionCallingTestClass
{
    public bool BoolParameter;
    public TestEnum EnumParameter;
    public TestEnum EnumParameter2;
    public float FloatParameter;
    public int IntParameter;
    public int? NotRequiredIntParameter;
    public int OverriddenNameParameter;
    public string OverriddenTypeParameter = null!;
    public int RequiredIntParameter;
    public string StringParameter = null!;

    [FunctionDescription("Test Function")]
    public int TestFunction(
        [ParameterDescription("Int Parameter")]
        int intParameter,
        [ParameterDescription("Float Parameter")]
        float floatParameter,
        [ParameterDescription("Bool Parameter")]
        bool boolParameter,
        [ParameterDescription("String Parameter")]
        string stringParameter,
        [ParameterDescription(Description = "Enum Parameter", Enum = "Value1, Value2, Value3")]
        TestEnum enumParameter,
        [ParameterDescription("Enum Parameter 2")]
        TestEnum enumParameter2,
        [ParameterDescription(Description = "Required Int Parameter", Required = true)]
        int requiredIntParameter,
        [ParameterDescription(Description = "Not required Int Parameter", Required = false)]
        int notRequiredIntParameter,
        [ParameterDescription(Name = "OverriddenName", Description = "Overridden")]
        int overriddenNameParameter)

    {
        IntParameter = intParameter;
        FloatParameter = floatParameter;
        BoolParameter = boolParameter;
        StringParameter = stringParameter;
        EnumParameter = enumParameter;
        EnumParameter2 = enumParameter2;
        RequiredIntParameter = requiredIntParameter;
        NotRequiredIntParameter = notRequiredIntParameter;
        OverriddenNameParameter = overriddenNameParameter;

        return 5;
    }

    [FunctionDescription("Second Function")]
    public string SecondFunction()
    {
        return "Hello";
    }

    [FunctionDescription("Third Function")]
    public void ThirdFunction([ParameterDescription(Type = "string", Description = "Overridden type parameter")] int overriddenTypeParameter)
    {
        OverriddenTypeParameter = overriddenTypeParameter.ToString();
    }
}

public enum TestEnum
{
    Value1,
    Value2,
    Value3
}