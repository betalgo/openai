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

        var enumValues = new List<string> { "Value1", "Value2", "Value3" };

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
        var functionDefinitions = FunctionCallingHelper.GetToolDefinitions<FunctionCallingTestClass>();

        functionDefinitions.Count.ShouldBe(4);

        var functionDefinition = functionDefinitions.First(x => x.Function!.Name == "TestFunction");
        functionDefinition.Function!.Description.ShouldBe("Test Function");
        functionDefinition.Function!.Parameters.ShouldNotBeNull();
        functionDefinition.Function!.Parameters.Properties!.Count.ShouldBe(9);

        var functionDefinition2 = functionDefinitions.First(x => x.Function!.Name == "SecondFunction");
        functionDefinition2.Function!.Description.ShouldBe("Second Function");
        functionDefinition2.Function!.Parameters.ShouldNotBeNull();
        functionDefinition2.Function!.Parameters.Properties!.Count.ShouldBe(0);

        var functionDefinition3 = functionDefinitions.First(x => x.Function!.Name == "ThirdFunction");
        functionDefinition3.Function!.Description.ShouldBe("Third Function");
        functionDefinition3.Function!.Parameters.ShouldNotBeNull();
        functionDefinition3.Function!.Parameters.Properties!.Count.ShouldBe(1);

        var functionDefinition4 = functionDefinitions.First(x => x.Function!.Name == "fourth_function");
        functionDefinition4.Function!.Description.ShouldBe("Fourth Function");
        functionDefinition4.Function!.Name.ShouldBe("fourth_function");
        functionDefinition4.Function!.Parameters.ShouldNotBeNull();
        functionDefinition4.Function!.Parameters.Properties!.Count.ShouldBe(0);
    }

    [Fact]
    public void VerifyCallFunction_CustomFunctionName()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "fourth_function"
        };

        var result = FunctionCallingHelper.CallFunction<string>(functionCall, obj);
        result.ShouldBe("Ciallo～(∠・ω< )⌒★");
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
            Arguments =
                "{\"intParameter\": \"invalid\", \"floatParameter\": true, \"boolParameter\": 1, \"stringParameter\": 123, \"enumParameter\": \"NonExistentValue\"}"
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


    [Fact]
    public void VerifyGetFunctionDefinition_CustomType()
    {
        var functionDefinition =
            FunctionCallingHelper.GetFunctionDefinition(
                typeof(FunctionCallingTestClass).GetMethod("FunctionWithCustomType")!);

        functionDefinition.Name.ShouldBe("FunctionWithCustomType");
        functionDefinition.Description.ShouldBe("Function with custom type parameter");
        functionDefinition.Parameters.ShouldNotBeNull();
        functionDefinition.Parameters.Properties!.Count.ShouldBe(1);

        var customTypeParameter = functionDefinition.Parameters.Properties["customTypeParameter"];
        customTypeParameter.Description.ShouldBe("Custom type parameter");
        customTypeParameter.Type.ShouldBe("object");

        customTypeParameter.Properties.ShouldNotBeNull();
        customTypeParameter.Properties!.Count.ShouldBe(4);

        var nameProperty = customTypeParameter.Properties["Name"];
        nameProperty.Type.ShouldBe("string");
        nameProperty.Description.ShouldBe("The name");

        var ageProperty = customTypeParameter.Properties["Age"];
        ageProperty.Type.ShouldBe("integer");
        ageProperty.Description.ShouldBe("The age");

        var scoreProperty = customTypeParameter.Properties["Score"];
        scoreProperty.Type.ShouldBe("number");
        scoreProperty.Description.ShouldBe("The score");

        var isActiveProperty = customTypeParameter.Properties["IsActive"];
        isActiveProperty.Type.ShouldBe("boolean");
        isActiveProperty.Description.ShouldBe("The status");
    }

    [Fact]
    public void VerifyGetFunctionDefinition_ComplexCustomType()
    {
        var functionDefinition =
            FunctionCallingHelper.GetFunctionDefinition(
                typeof(FunctionCallingTestClass).GetMethod("FunctionWithComplexCustomType")!);


        functionDefinition.Name.ShouldBe("FunctionWithComplexCustomType");
        functionDefinition.Description.ShouldBe("Function with complex custom type parameter");
        functionDefinition.Parameters.ShouldNotBeNull();
        functionDefinition.Parameters.Properties!.Count.ShouldBe(1);

        var complexCustomTypeParameter = functionDefinition.Parameters.Properties["complexCustomTypeParameter"];
        complexCustomTypeParameter.Description.ShouldBe("Complex custom type parameter");
        complexCustomTypeParameter.Type.ShouldBe("object");

        complexCustomTypeParameter.Properties.ShouldNotBeNull();
        complexCustomTypeParameter.Properties!.Count.ShouldBe(5);

        complexCustomTypeParameter.Properties.ShouldContainKey("Name");
        complexCustomTypeParameter.Properties.ShouldContainKey("Age");
        complexCustomTypeParameter.Properties.ShouldContainKey("Scores");
        complexCustomTypeParameter.Properties.ShouldContainKey("IsActive");
        complexCustomTypeParameter.Properties.ShouldContainKey("NestedCustomType");

        var nestedCustomTypeProperty = complexCustomTypeParameter.Properties["NestedCustomType"];
        nestedCustomTypeProperty.Type.ShouldBe("object");

        nestedCustomTypeProperty.Properties.ShouldNotBeNull();
        nestedCustomTypeProperty.Properties!.Count.ShouldBe(4);

        nestedCustomTypeProperty.Properties.ShouldContainKey("Name");
        nestedCustomTypeProperty.Properties.ShouldContainKey("Age");
        nestedCustomTypeProperty.Properties.ShouldContainKey("Score");
        nestedCustomTypeProperty.Properties.ShouldContainKey("IsActive");
    }

    [Fact]
    public void VerifyCallFunction_ComplexCustomType()
    {
        var obj = new FunctionCallingTestClass();

        var functionCall = new FunctionCall
        {
            Name = "FunctionWithComplexCustomType",
            Arguments =
                "{\"complexCustomTypeParameter\": {\"Name\": \"John\", \"Age\": 30, \"Scores\": [85.5, 92.0, 78.5], \"IsActive\": true, \"NestedCustomType\": {\"Name\": \"Nested\", \"Age\": 20, \"Score\": 95.0, \"IsActive\": false}}}"
        };

        FunctionCallingHelper.CallFunction<object>(functionCall, obj);

        obj.ComplexCustomTypeParameter.ShouldNotBeNull();
        obj.ComplexCustomTypeParameter.Name.ShouldBe("John");
        obj.ComplexCustomTypeParameter.Age.ShouldBe(30);
        obj.ComplexCustomTypeParameter.Scores.ShouldBe(new List<float> { 85.5f, 92.0f, 78.5f });
        obj.ComplexCustomTypeParameter.IsActive.ShouldBe(true);

        obj.ComplexCustomTypeParameter.NestedCustomType.ShouldNotBeNull();
        obj.ComplexCustomTypeParameter.NestedCustomType.Name.ShouldBe("Nested");
        obj.ComplexCustomTypeParameter.NestedCustomType.Age.ShouldBe(20);
        obj.ComplexCustomTypeParameter.NestedCustomType.Score.ShouldBe(95.0f);
        obj.ComplexCustomTypeParameter.NestedCustomType.IsActive.ShouldBe(false);
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
    public ComplexCustomType ComplexCustomTypeParameter { get; set; } = new();

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
    public void ThirdFunction(
        [ParameterDescription(Type = "string", Description = "Overridden type parameter")]
        int overriddenTypeParameter)
    {
        OverriddenTypeParameter = overriddenTypeParameter.ToString();
    }

    [FunctionDescription("Fourth Function", Name = "fourth_function")]
    public string FourthFunction()
    {
        return "Ciallo～(∠・ω< )⌒★";
    }

    [FunctionDescription("Function with complex custom type parameter")]
    public void FunctionWithComplexCustomType(
        [ParameterDescription("Complex custom type parameter")]
        ComplexCustomType complexCustomTypeParameter)
    {
        ComplexCustomTypeParameter = complexCustomTypeParameter;
    }
}

public class ComplexCustomType
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public List<float> Scores { get; set; } = new();

    public bool IsActive { get; set; }

    public CustomType NestedCustomType { get; set; } = new();
}

public class CustomType
{
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public float Score { get; set; }

    public bool IsActive { get; set; }
}

public enum TestEnum
{
    Value1,
    Value2,
    Value3
}