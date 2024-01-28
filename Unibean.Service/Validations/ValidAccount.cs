using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidAccount : ValidationAttribute
{
    private new const string ErrorMessage = "Tài khoản không hợp lệ";

    private readonly IAccountRepository accountRepo = new AccountRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var acc = accountRepo.GetById(value.ToString());
        if (acc != null && (bool)acc.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
