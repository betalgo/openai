using System.Reflection;
using System.Text.Json;
using OpenAI.Builders;
using OpenAI.ObjectModels.RequestModels;
using OpenAI.ObjectModels.SharedModels;

namespace OpenAI.Utilities;

/// <summary>
/// Helper methods for Function Calling
/// </summary>
public static class FunctionCallingHelper
{
	/// <summary>
	/// Returns a <see cref="FunctionDefinition"/> from the provided method, using any <see cref="FunctionDescriptionAttribute"/> and <see cref="ParameterDescriptionAttribute"/> attributes
	/// </summary>
	/// <param name="methodInfo">the method to create the <see cref="FunctionDefinition"/> from</param>
	/// <returns>the <see cref="FunctionDefinition"/> created.</returns>
	public static FunctionDefinition GetFunctionDefinition(MethodInfo methodInfo)
	{
		var methodDescriptionAttribute = methodInfo.GetCustomAttribute<FunctionDescriptionAttribute>();

		var result = new FunctionDefinitionBuilder(
			methodDescriptionAttribute?.Name ?? methodInfo.Name, methodDescriptionAttribute?.Description);

		var parameters = methodInfo.GetParameters().ToList();

		foreach (var parameter in parameters)
		{
			var parameterDescriptionAttribute = parameter.GetCustomAttribute<ParameterDescriptionAttribute>();
			var description = parameterDescriptionAttribute?.Description;

			PropertyDefinition definition;

			switch (parameter.ParameterType)
			{
				case { } t when t.IsAssignableFrom(typeof(int)):
					definition = PropertyDefinition.DefineInteger(description);
					break;
				case {  } t when t.IsAssignableFrom(typeof(float)):
					definition = PropertyDefinition.DefineNumber(description);
					break;
				case { } t when t.IsAssignableFrom(typeof(bool)):
					definition = PropertyDefinition.DefineBoolean(description);
					break;
				case { } t when t.IsAssignableFrom(typeof(string)):
					definition = PropertyDefinition.DefineString(description);
					break;
				case { IsEnum: true }:

					var enumValues = string.IsNullOrEmpty(parameterDescriptionAttribute?.Enum)
						? Enum.GetNames(parameter.ParameterType).ToList()
						: parameterDescriptionAttribute!.Enum!.Split(",").Select(x => x.Trim()).ToList();
						

					definition =
						PropertyDefinition.DefineEnum(enumValues, description);
					break;
				default:
					throw new Exception($"Parameter type '{parameter.ParameterType}' not supported");
			}

			result.AddParameter(
				parameterDescriptionAttribute?.Name ?? parameter.Name!,
				definition,
				parameterDescriptionAttribute?.Required ?? true);
		}

		return result.Build();
	}

	/// <summary>
	/// Enumerates the methods in the provided object, and a returns a <see cref="List{FunctionDefinition}"/> of <see cref="FunctionDefinition"/> for all methods
	/// marked with a <see cref="FunctionDescriptionAttribute"/>
	/// </summary>
	/// <param name="obj">the object to analyze</param>
	public static List<FunctionDefinition> GetFunctionDefinitions(object obj)
	{
		var type = obj.GetType();
		return GetFunctionDefinitions(type);
	}

	/// <summary>
	/// Enumerates the methods in the provided type, and a returns a <see cref="List{FunctionDefinition}"/> of <see cref="FunctionDefinition"/> for all methods
	/// </summary>
	/// <param name="type">The type to analyze</param>
	public static List<FunctionDefinition> GetFunctionDefinitions(Type type)
	{
		var methods = type.GetMethods();

		var result = methods
			.Select(method => new
			{
				method,
				methodDescriptionAttribute = method.GetCustomAttribute<FunctionDescriptionAttribute>()
			})
			.Where(@t => @t.methodDescriptionAttribute != null)
			.Select(@t => GetFunctionDefinition(@t.method)).ToList();

		return result;
	}

	/// <summary>
	/// Calls the function on the provided object, using the provided <see cref="FunctionCall"/> and returns the result of the call
	/// </summary>
	/// <param name="functionCall">The FunctionCall provided by the LLM</param>
	/// <param name="obj">the object with the method / function to be executed</param>
	/// <typeparam name="T">The return type</typeparam>
	public static T? CallFunction<T>(FunctionCall functionCall, object obj)
	{
		if (functionCall == null)
			throw new ArgumentNullException(nameof(functionCall));

		if (functionCall.Name == null)
			throw new Exception("Function name is null");

		var methodInfo = obj.GetType().GetMethod(functionCall.Name);

		if (methodInfo == null)
			throw new Exception($"Method '{functionCall.Name}' on type '{obj.GetType()}' not found");

		if (!methodInfo.ReturnType.IsAssignableTo(typeof(T)))
			throw new Exception(
				$"Method '{functionCall.Name}' on type '{obj.GetType()}' has return type '{methodInfo.ReturnType}' but expected '{typeof(T)}'");

		var parameters = methodInfo.GetParameters().ToList();
		var arguments = functionCall.ParseArguments();
		var args = new List<object?>();

		foreach (var parameter in parameters)
		{
			ParameterDescriptionAttribute? parameterDescriptionAttribute =
				parameter.GetCustomAttribute<ParameterDescriptionAttribute>();

			var name = parameterDescriptionAttribute?.Name ?? parameter.Name!;
			var argument = arguments.FirstOrDefault(x => x.Key == name);

			if (argument.Key == null)
				throw new Exception($"Argument '{name}' not found");

			var value = parameter.ParameterType.IsEnum ?
				Enum.Parse(parameter.ParameterType, argument.Value.ToString()!) : 
				((JsonElement)argument.Value).Deserialize(parameter.ParameterType);
			
			args.Add(value);
		}

		T? result = (T?)methodInfo.Invoke(obj, args.ToArray());
		return result;
	}
}

/// <summary>
/// Attribute to mark a method as a function, and provide a description for the function. Can also be used to override the Name of the function
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class FunctionDescriptionAttribute : Attribute
{
	/// <summary>
	/// Name of the function. If not provided, the name of the method will be used.
	/// </summary>
	public string? Name { get; set; }
	/// <summary>
	/// Description of the function
	/// </summary>
	public string? Description { get; set; }

	/// <summary>
	/// Creates a new instance of the <see cref="FunctionDescriptionAttribute"/> with the provided description
	/// </summary>
	public FunctionDescriptionAttribute(string? description = null)
	{
		Description = description;
	}
}

/// <summary>
/// Attribute to describe a parameter of a function. Can also be used to override the Name of the parameter
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class ParameterDescriptionAttribute : Attribute
{
	/// <summary>
	/// Name of the parameter. If not provided, the name of the parameter will be used.
	/// </summary>
	public string? Name { get; set; }
	/// <summary>
	/// Description of the parameter
	/// </summary>
	public string? Description { get; set; }
	/// <summary>
	/// Type of the parameter. If not provided, the type of the parameter will be inferred from the parameter type
	/// </summary>
	public string? Type { get; set; }
	/// <summary>
	/// Enum values of the parameter in a comma separated string. If not provided, the enum values will be inferred from the parameter type
	/// </summary>
	public string? Enum { get; set; }
	/// <summary>
	/// Whether the parameter is required. If not provided, the parameter will be required. Default is true
	/// </summary>
	public bool Required { get; set; } = true;

	/// <summary>
	/// Creates a new instance of the <see cref="ParameterDescriptionAttribute"/> with the provided description
	/// </summary>
	public ParameterDescriptionAttribute(string? description = null)
	{
		Description = description;
	}
}

