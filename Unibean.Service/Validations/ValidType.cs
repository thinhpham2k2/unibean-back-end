using System.ComponentModel.DataAnnotations;
using Type = Unibean.Repository.Entities.Type;

namespace Unibean.Service.Validations;

public class ValidType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại hoạt động không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int type))
        {
            if (Enum.IsDefined(typeof(Type), type))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
