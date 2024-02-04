using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidRole : ValidationAttribute
{
    private new const string ErrorMessage = "Vai trò không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int role))
        {
            if (Enum.IsDefined(typeof(Role), role))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
