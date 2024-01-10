using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidBrand : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid brand"; 
    
    private readonly IBrandRepository brandRepo = new BrandRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (brandRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
