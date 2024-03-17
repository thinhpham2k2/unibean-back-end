using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidMajor : ValidationAttribute
{
    private new const string ErrorMessage = "Chuyên ngành không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var majorRepo = validationContext.GetService<IMajorRepository>();
        var major = majorRepo.GetById(value.ToString());
        if (major != null && (bool)major.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
