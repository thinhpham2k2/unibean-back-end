using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCity : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid city"; 
    
    private readonly ICityRepository cityRepo = new CityRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (cityRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
