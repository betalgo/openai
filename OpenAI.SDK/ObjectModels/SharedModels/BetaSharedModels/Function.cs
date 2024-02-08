using System.Text.Json.Serialization;

namespace OpenAI.ObjectModels.SharedModels.BetaSharedModels;

public class Function
{
  /// <summary>
  /// A description of what the function does, used by the model to choose when and how to call the function.
  /// </summary>
  [JsonPropertyName("description")]
  public string Description { get; set; }   
  
  /// <summary>
  /// The name of the function to be called.
  /// </summary>
  [JsonPropertyName("name")]
  public string Name { get; set; }

  /// <summary>
  /// The parameters the functions accepts, described as a JSON Schema object.
  /// </summary>
  [JsonPropertyName("parameters")] 
  public Dictionary<string, object> Parameters;
}