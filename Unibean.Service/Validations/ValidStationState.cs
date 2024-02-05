using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidStationState : ValidationAttribute
{
    private new const string ErrorMessage = "Trạng thái trạm không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int state))
        {
            if (Enum.IsDefined(typeof(StationState), state))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
