using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidMajor : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid major";

    private readonly IMajorRepository majorRepo = new MajorRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (majorRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
