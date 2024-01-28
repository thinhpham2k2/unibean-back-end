using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStation : ValidationAttribute
{
    private new const string ErrorMessage = "Trạm không hợp lệ"; 
    
    private readonly IStationRepository stationRepo = new StationRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var station = stationRepo.GetById(value.ToString());
        if (station != null && (bool)station.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
