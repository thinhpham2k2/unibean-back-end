using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using System.Text.RegularExpressions;

namespace Unibean.Service.Validations;

public class ValidPhone : ValidationAttribute
{
    private new const string ErrorMessage = "Số điện thoại không hợp lệ";

    private const string ErrorMessage1 = "Số điện thoại đã được sử dụng";

    private readonly IAccountRepository accountRepository = new AccountRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string phone = value.ToString();
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
