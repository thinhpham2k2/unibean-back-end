using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampaign : ValidationAttribute
{
    private readonly ICampaignRepository campaignRepo = new CampaignRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (campaignRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid campaign");
    }
}
