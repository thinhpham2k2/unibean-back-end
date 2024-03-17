using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidCampaignDetailId : ValidationAttribute
{
    private new const string ErrorMessage = "Chi tiết chiến dịch không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var detailRepo = validationContext.GetService<ICampaignDetailRepository>();
        var detail = detailRepo.GetById(value.ToString());
        if (detail != null && (bool)detail.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
