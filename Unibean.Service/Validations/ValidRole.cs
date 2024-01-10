using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidRole : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid role";

    private readonly IRoleRepository roleRepository = new RoleRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (roleRepository.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
