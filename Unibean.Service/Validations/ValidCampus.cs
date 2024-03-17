using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidCampus : ValidationAttribute
{
    private new const string ErrorMessage = "Cơ sở không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var campusRepo = validationContext.GetService<ICampusRepository>();
        var cam = campusRepo.GetById(value.ToString());
        if (cam != null && (bool)cam.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
