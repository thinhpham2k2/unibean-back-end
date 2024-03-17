using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidUniversity : ValidationAttribute
{
    private new const string ErrorMessage = "Trường đại học không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var universityRepo = validationContext.GetService<IUniversityRepository>();
        var uni = universityRepo.GetById(value.ToString());
        if (uni != null && (bool)uni.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
