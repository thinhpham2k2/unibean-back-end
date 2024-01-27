using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampaignType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại chiến dịch không hợp lệ";

    private readonly ICampaignTypeRepository campaignTypeRepo = new CampaignTypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var type = campaignTypeRepo.GetById(value.ToString());
        if (type != null && (bool)type.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
