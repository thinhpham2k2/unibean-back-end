using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidEmail : ValidationAttribute
{
    private new const string ErrorMessage = "Email đã được sử dụng";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var accountRepository = validationContext.GetService<IAccountRepository>();
        string email = value.ToString();
        if (accountRepository.CheckEmailDuplicate(email))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
