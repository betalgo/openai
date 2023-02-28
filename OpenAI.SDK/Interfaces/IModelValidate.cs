using System.ComponentModel.DataAnnotations;

namespace OpenAI.GPT3.Interfaces;

public interface IModelValidate
{
    IEnumerable<ValidationResult> Validate();
}