using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidDistrict : ValidationAttribute
{
    private readonly IDistrictRepository districtRepo = new DistrictRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (districtRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid district");
    }
}
