using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidAdmin : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid admin";

    private readonly IAdminRepository adminRepo = new AdminRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (adminRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
