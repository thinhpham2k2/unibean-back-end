using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidEndOn : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid end day";

    private const string ErrorMessage1 = "The end date must be some day from the start date";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (DateOnly.TryParse(value.ToString(), out DateOnly EndOn))
            {
                if (EndOn >= create.StartOn)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage1);
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
