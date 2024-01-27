using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidWalletType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại ví không hợp lệ";

    private readonly IWalletTypeRepository typeRepo = new WalletTypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var type = typeRepo.GetById(value.ToString());
        if (type != null && (bool)type.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
