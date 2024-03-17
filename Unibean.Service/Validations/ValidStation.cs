using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidStation : ValidationAttribute
{
    private readonly StationState[] states;

    public ValidStation(StationState[] states)
    {
        this.states = states;
    }

    private new const string ErrorMessage = "Trạm không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var stationRepo = validationContext.GetService<IStationRepository>();
        var station = stationRepo.GetById(value.ToString());
        if (station != null && states.Contains(station.State.Value))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
