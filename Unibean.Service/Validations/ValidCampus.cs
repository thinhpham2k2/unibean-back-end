using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampus : ValidationAttribute
{
    private readonly ICampusRepository campusRepo = new CampusRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (campusRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid campus");
    }
}
