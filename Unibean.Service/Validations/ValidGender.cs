using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidGender : ValidationAttribute
{
    private readonly IGenderRepository genderRepo = new GenderRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (genderRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid gender");
    }
}
