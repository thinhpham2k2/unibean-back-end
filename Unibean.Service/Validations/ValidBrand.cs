using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidBrand : ValidationAttribute
{
    private new const string ErrorMessage = "Thương hiệu không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var brandRepo = validationContext.GetService<IBrandRepository>();
        var brand = brandRepo.GetById(value.ToString());
        if (brand != null && (bool)brand.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
