using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStore : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid store";

    private readonly IStoreRepository storeRepo = new StoreRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (storeRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
