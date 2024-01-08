using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidState : ValidationAttribute
{
    private readonly IStateRepository stateRepo = new StateRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (stateRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid state");
    }
}
