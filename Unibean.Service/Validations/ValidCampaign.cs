using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidCampaign : ValidationAttribute
{
    private readonly CampaignState[] states;

    public ValidCampaign(CampaignState[] states)
    {
        this.states = states;
    }

    private new const string ErrorMessage = "Chiến dịch không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var campaignRepo = validationContext.GetService<ICampaignRepository>();
        var cam = campaignRepo.GetById(value.ToString());
        if (cam != null && states.Contains(cam.CampaignActivities.LastOrDefault().State.Value))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
