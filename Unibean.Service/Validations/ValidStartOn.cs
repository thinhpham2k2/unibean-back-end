using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Validations;

public class ValidStartOn : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid start day";

    private const string ErrorMessage1 = "The start date must be one day from today onward";

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
