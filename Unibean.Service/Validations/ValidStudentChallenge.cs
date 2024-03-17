using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidStudentChallenge : ValidationAttribute
{
    private new const string ErrorMessage = "Thử thách không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var studentChallengeRepo = validationContext.GetService<IStudentChallengeRepository>();
        var chall = studentChallengeRepo.GetById(value.ToString());
        if (chall != null && (bool)chall.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
