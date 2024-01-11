using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidGender : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid gender";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int gender))
        {
            if (new List<int> { 1, 2 }.Contains(gender))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
