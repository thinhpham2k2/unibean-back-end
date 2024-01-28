using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidAdmin : ValidationAttribute
{
    private new const string ErrorMessage = "Quản trị viên không hợp lệ";

    private readonly IAdminRepository adminRepo = new AdminRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var admin = adminRepo.GetById(value.ToString());
        if (admin != null && (bool)admin.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
