using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidCampaign : ValidationAttribute
{
    private readonly CampaignState[] states;

    public ValidCampaign(CampaignState[] states)
    {
        this.states = states;
    }

    private new const string ErrorMessage = "Chiến dịch không hợp lệ"; 
    
    private readonly ICampaignRepository campaignRepo = new CampaignRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cam = campaignRepo.GetById(value.ToString());
        if (cam != null && states.Contains(cam.CampaignActivities.LastOrDefault().State.Value))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
