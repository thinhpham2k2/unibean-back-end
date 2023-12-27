using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidUniversity : ValidationAttribute
{
    private readonly IUniversityRepository universityRepo = new UniversityRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (universityRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid university");
    }
}
