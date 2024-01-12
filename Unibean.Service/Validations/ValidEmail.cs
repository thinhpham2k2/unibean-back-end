using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidEmail : ValidationAttribute
{
    private new const string ErrorMessage = "Email is already in use";

    private readonly IAccountRepository accountRepository = new AccountRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string email = value.ToString();
        if (accountRepository.CheckEmailDuplicate(email))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
