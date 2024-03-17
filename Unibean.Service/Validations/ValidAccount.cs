using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidAccount : ValidationAttribute
{
    private new const string ErrorMessage = "Tài khoản không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var accountRepo = validationContext.GetService<IAccountRepository>();
        var acc = accountRepo.GetById(value.ToString());
        if (acc != null && (bool)acc.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
