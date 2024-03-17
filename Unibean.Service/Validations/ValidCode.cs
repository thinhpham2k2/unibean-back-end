using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidCode : ValidationAttribute
{
    private new const string ErrorMessage = "Mã sinh viên đã được sử dụng";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var studentRepo = validationContext.GetService<IStudentRepository>();
        string code = value.ToString();
        if (studentRepo.CheckCodeDuplicate(code))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
