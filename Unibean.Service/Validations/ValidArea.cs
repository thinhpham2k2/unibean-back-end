using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidArea : ValidationAttribute
{
    private new const string ErrorMessage = "Khu vực không hợp lệ"; 
    
    private readonly IAreaRepository areaRepo = new AreaRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var area = areaRepo.GetById(value.ToString());
        if (area != null && (bool)area.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
