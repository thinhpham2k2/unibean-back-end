using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidUserName : ValidationAttribute
{
    private readonly IAccountRepository accountRepository = new AccountRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string userName = value.ToString();
        if (Regex.IsMatch(userName, @"^[a-z0-9]{5,50}$"))
        {
            if (accountRepository.CheckUsernameDuplicate(userName))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("User name is already in use");
        }
        return new ValidationResult("Username must contain lowercase " +
                    "letters or numbers, and be between 5 and 50 characters in length");
    }
}
