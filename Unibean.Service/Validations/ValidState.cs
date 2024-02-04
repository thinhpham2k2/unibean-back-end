using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidState : ValidationAttribute
{
    private new const string ErrorMessage = "Trạng thái đơn hàng không hợp lệ";
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int state))
        {
            if (Enum.IsDefined(typeof(State), state))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
