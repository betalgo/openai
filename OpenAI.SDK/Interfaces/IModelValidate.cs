using System.ComponentModel.DataAnnotations;

namespace Betalgo.OpenAI.Interfaces;

public interface IModelValidate
{
    IEnumerable<ValidationResult> Validate();
}