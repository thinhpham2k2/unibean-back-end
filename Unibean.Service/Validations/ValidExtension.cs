using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidExtension : ValidationAttribute
{
    private readonly string[] _extensions;

    private new const string ErrorMessage = "Tệp không hợp lệ";

    public ValidExtension(string[] extensions)
    {
        _extensions = extensions;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var file = value as IFormFile;
        var extension = Path.GetExtension(file?.FileName);
        if (file != null)
        {
            if (!_extensions.Contains(extension.ToLower()) && file.Length <= 31457280)
            {
                return new ValidationResult(ErrorMessage + " (Chỉ chấp nhận các tệp thuộc loại " + string.Join(", ", _extensions) + ")");
            }
        }
        return ValidationResult.Success;
    }
}
