using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidUniversity : ValidationAttribute
{
    private new const string ErrorMessage = "Trường đại học không hợp lệ"; 
    
    private readonly IUniversityRepository universityRepo = new UniversityRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var uni = universityRepo.GetById(value.ToString());
        if (uni != null && (bool)uni.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
