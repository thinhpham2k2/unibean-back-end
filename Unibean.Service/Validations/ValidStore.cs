using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidStore : ValidationAttribute
{
    private new const string ErrorMessage = "Cửa hàng không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var storeRepo = validationContext.GetService<IStoreRepository>();
        var store = storeRepo.GetById(value.ToString());
        if (store != null && (bool)store.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
