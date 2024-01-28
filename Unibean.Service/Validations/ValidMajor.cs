using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidMajor : ValidationAttribute
{
    private new const string ErrorMessage = "Chuyên ngành không hợp lệ";

    private readonly IMajorRepository majorRepo = new MajorRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var major = majorRepo.GetById(value.ToString());
        if (major != null && (bool)major.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
