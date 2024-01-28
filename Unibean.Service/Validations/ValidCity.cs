using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCity : ValidationAttribute
{
    private new const string ErrorMessage = "Thành phố không hợp lệ"; 
    
    private readonly ICityRepository cityRepo = new CityRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var city = cityRepo.GetById(value.ToString());
        if (city != null && (bool)city.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
