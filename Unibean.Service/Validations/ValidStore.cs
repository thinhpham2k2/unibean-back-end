using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStore : ValidationAttribute
{
    private new const string ErrorMessage = "Cửa hàng không hợp lệ";

    private readonly IStoreRepository storeRepo = new StoreRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var store = storeRepo.GetById(value.ToString());
        if (store != null && (bool)store.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
