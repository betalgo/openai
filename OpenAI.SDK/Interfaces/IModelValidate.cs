using System.ComponentModel.DataAnnotations;

namespace Betalgo.Ranul.OpenAI.Interfaces;

public interface IModelValidate
{
    IEnumerable<ValidationResult> Validate();
}