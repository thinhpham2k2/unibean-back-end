using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidArea : ValidationAttribute
{
    private new const string ErrorMessage = "Khu vực không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var areaRepo = validationContext.GetService<IAreaRepository>();
        var area = areaRepo.GetById(value.ToString());
        if (area != null && (bool)area.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
