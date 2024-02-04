using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidWalletType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại ví không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int type))
        {
            if (Enum.IsDefined(typeof(WalletType), type))
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
