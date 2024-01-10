using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidBirthday : ValidationAttribute
{
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
                return new ValidationResult("Valid age must be greater than 18 and less than 100");
            }
        }
        return new ValidationResult("Invalid birthday");
    }
}
