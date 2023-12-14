using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidType : ValidationAttribute
{
    private readonly ITypeRepository typeRepo = new TypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (typeRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid activity's type");
    }
}
