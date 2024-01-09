using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStation : ValidationAttribute
{
    private readonly IStationRepository stationRepo = new StationRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (stationRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid station");
    }
}
