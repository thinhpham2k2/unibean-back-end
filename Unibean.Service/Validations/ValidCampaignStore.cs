using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Validations;

namespace Unibean.Service.Validations;

public class ValidCampaignStore : ValidationAttribute
{
    private new const string ErrorMessage = "Danh sách cửa hàng không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.CampaignStores != null)
            {
                List<string> storeIds = create.CampaignStores.Select(c => c.StoreId).ToList();
                if (storeIds.Count.Equals(storeIds.Distinct().ToList().Count) && storeIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        else if (validationContext.ObjectInstance is CampaignMSCModel verify)
        {
            if (verify.CampaignStores != null)
            {
                List<string> storeIds = verify.CampaignStores.Select(c => c.StoreId).ToList();
                if (storeIds.Count.Equals(storeIds.Distinct().ToList().Count) && storeIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
