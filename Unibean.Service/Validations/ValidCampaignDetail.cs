using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidCampaignDetail : ValidationAttribute
{
    private new const string ErrorMessage = "Chi tiết chiến dịch không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.CampaignDetails != null)
            {
                List<string> voucherIds = create.CampaignDetails.Select(c => c.VoucherId).ToList();
                if (voucherIds.Count.Equals(voucherIds.Distinct().ToList().Count) && voucherIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
