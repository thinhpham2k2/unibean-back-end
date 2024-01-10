using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidAccount : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid account";
    private readonly IAccountRepository accountRepo = new AccountRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (accountRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
