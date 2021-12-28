using System.ComponentModel.DataAnnotations;

namespace OpenAI.SDK.Interfaces
{
    public interface IModelValidate
    {
        IEnumerable<ValidationResult> Validate();
    }
}