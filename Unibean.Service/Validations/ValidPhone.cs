using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidPhone : ValidationAttribute
{
    private new const string ErrorMessage = "Số điện thoại không hợp lệ";

    private const string ErrorMessage1 = "Số điện thoại đã được sử dụng";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string phone = value.ToString();
        var accountRepository = validationContext.GetService<IAccountRepository>();
        if (Regex.IsMatch(phone, @"\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})"))
        {
            if (accountRepository.CheckPhoneDuplicate(phone))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage1);
        }
        return new ValidationResult(ErrorMessage);
    }
}
