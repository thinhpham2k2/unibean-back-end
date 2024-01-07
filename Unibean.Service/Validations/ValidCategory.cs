using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCategory : ValidationAttribute
{
    private readonly ICategoryRepository categoryRepo = new CategoryRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (categoryRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid category");
    }
}
