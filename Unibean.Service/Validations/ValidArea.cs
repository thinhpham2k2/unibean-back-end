using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidArea : ValidationAttribute
{
    private readonly IAreaRepository areaRepo = new AreaRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (areaRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid area");
    }
}
