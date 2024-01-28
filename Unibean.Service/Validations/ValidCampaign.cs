using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampaign : ValidationAttribute
{
    private new const string ErrorMessage = "Chiến dịch không hợp lệ"; 
    
    private readonly ICampaignRepository campaignRepo = new CampaignRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cam = campaignRepo.GetById(value.ToString());
        if (cam != null && (bool)cam.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
