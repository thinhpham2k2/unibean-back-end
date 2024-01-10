using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using System.Text.RegularExpressions;

namespace Unibean.Service.Validations;

public class ValidPhone : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid phone number";

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
            return new ValidationResult("Phone is already in use");
        }
        return new ValidationResult(ErrorMessage);
    }
}
