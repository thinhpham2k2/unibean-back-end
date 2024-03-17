using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidInviteCode : ValidationAttribute
{
    private new const string ErrorMessage = "Mã mời không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var studentRepo = validationContext.GetService<IStudentRepository>();
        string inviteCode = value?.ToString();
        if (inviteCode.IsNullOrEmpty() || studentRepo.CheckInviteCode(inviteCode))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
