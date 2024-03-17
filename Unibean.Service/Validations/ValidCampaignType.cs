using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidCampaignType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại chiến dịch không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var campaignTypeRepo = validationContext.GetService<ICampaignTypeRepository>();
        var type = campaignTypeRepo.GetById(value.ToString());
        if (type != null && (bool)type.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
