using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Validations;

namespace Unibean.Service.Validations;

public class ValidEndOn : ValidationAttribute
{
    private new const string ErrorMessage = "Ngày kết thúc không hợp lệ";

    private const string ErrorMessage1 = "Ngày kết thúc phải có sau hoặc cùng ngày bắt đầu";

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
        else if (validationContext.ObjectInstance is TimeModel time)
        {

            if (DateOnly.TryParse(value.ToString(), out DateOnly EndOn))
            {
                if (EndOn >= time.StartOn)
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage1);
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
