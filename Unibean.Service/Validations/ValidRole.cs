using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidRole : ValidationAttribute
{
    private new const string ErrorMessage = "Vai trò không hợp lệ";

    private readonly IRoleRepository roleRepository = new RoleRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var role = roleRepository.GetById(value.ToString());
        if (role != null && (bool)role.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
