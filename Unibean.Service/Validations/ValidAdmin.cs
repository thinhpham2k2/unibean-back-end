using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidAdmin : ValidationAttribute
{
    private new const string ErrorMessage = "Quản trị viên không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var adminRepo = validationContext.GetService<IAdminRepository>();
        var admin = adminRepo.GetById(value.ToString());
        if (admin != null && (bool)admin.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
