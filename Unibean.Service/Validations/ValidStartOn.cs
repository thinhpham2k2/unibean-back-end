using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidStartOn : ValidationAttribute
{
    private new const string ErrorMessage = "Ngày bắt đầu không hợp lệ";

    private const string ErrorMessage1 = "Ngày bắt đầu phải là một ngày kể từ hôm nay trở đi";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (DateOnly.TryParse(value.ToString(), out DateOnly StartOn))
        {
            if (StartOn >= DateOnly.FromDateTime(DateTime.Now))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage1);
        }
        return new ValidationResult(ErrorMessage);
    }
}
