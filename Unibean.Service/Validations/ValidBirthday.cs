using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidBirthday : ValidationAttribute
{
    private new const string ErrorMessage = "Độ tuổi hợp lệ phải lớn hơn 18 và nhỏ hơn 100";

    private const string ErrorMessage1 = "Sinh nhật không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (DateTime.TryParse(value.ToString(), out DateTime birthday))
        {
            if (birthday > DateTime.Now.AddYears(-100) && birthday < DateTime.Now.AddYears(-18))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }
        return new ValidationResult(ErrorMessage1);
    }
}
