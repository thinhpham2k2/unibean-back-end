using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidUserName : ValidationAttribute
{
    private new const string ErrorMessage = "Tên đăng nhập phải chứa chữ cái " +
        "hoặc số viết thường và có độ dài từ 5 đến 50 ký tự";

    private const string ErrorMessage1 = "Tên đăng nhập đã được sử dụng";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var accountRepository = validationContext.GetService<IAccountRepository>();
        string userName = value.ToString();
        if (Regex.IsMatch(userName, @"^[a-z0-9]{5,50}$"))
        {
            if (accountRepository.CheckUsernameDuplicate(userName))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage1);
        }
        return new ValidationResult(ErrorMessage);
    }
}
