using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampaignType : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid campaign type";

    private readonly ICampaignTypeRepository campaignTypeRepo = new CampaignTypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (campaignTypeRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
