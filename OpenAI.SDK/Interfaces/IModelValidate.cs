using System.ComponentModel.DataAnnotations;

namespace OpenAI.Interfaces;

public interface IModelValidate
{
    IEnumerable<ValidationResult> Validate();
}