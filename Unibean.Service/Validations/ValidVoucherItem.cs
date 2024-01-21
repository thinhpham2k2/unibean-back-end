using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidVoucherItem : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid vouchers";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.Vouchers != null)
            {
                List<string> voucherIds = create.Vouchers.Select(c => c.VoucherId).ToList();
                if (voucherIds.Count.Equals(voucherIds.Distinct().ToList().Count) && voucherIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
