using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidDistrict : ValidationAttribute
{
    private new const string ErrorMessage = "Quận không hợp lệ"; 
    
    private readonly IDistrictRepository districtRepo = new DistrictRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dis = districtRepo.GetById(value.ToString());
        if (dis != null && (bool)dis.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
