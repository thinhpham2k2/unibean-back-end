using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampus : ValidationAttribute
{
    private new const string ErrorMessage = "Cơ sở không hợp lệ"; 
    
    private readonly ICampusRepository campusRepo = new CampusRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cam = campusRepo.GetById(value.ToString());
        if (cam != null && (bool)cam.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
