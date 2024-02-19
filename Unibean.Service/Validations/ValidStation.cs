using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidStation : ValidationAttribute
{
    private readonly StationState[] states;

    public ValidStation(StationState[] states)
    {
        this.states = states;
    }

    private new const string ErrorMessage = "Trạm không hợp lệ"; 
    
    private readonly IStationRepository stationRepo = new StationRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var station = stationRepo.GetById(value.ToString());
        if (station != null && states.Contains(station.State.Value))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
