using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidLevel : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid level";

    private readonly ILevelRepository levelRepo = new LevelRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (levelRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
