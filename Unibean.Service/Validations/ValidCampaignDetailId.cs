using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCampaignDetailId : ValidationAttribute
{
    private new const string ErrorMessage = "Chi tiết chiến dịch không hợp lệ";

    private readonly ICampaignDetailRepository detailRepo = new CampaignDetailRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var detail = detailRepo.GetById(value.ToString());
        if (detail != null && (bool)detail.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
