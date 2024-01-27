using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidBrand : ValidationAttribute
{
    private new const string ErrorMessage = "Thương hiệu không hợp lệ"; 
    
    private readonly IBrandRepository brandRepo = new BrandRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var brand = brandRepo.GetById(value.ToString());
        if (brand != null && (bool)brand.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
