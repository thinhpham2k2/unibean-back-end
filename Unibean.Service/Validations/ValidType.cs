using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidType : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid activity type"; 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int type))
        {
            if (new List<int> { 1, 2 }.Contains(type))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
