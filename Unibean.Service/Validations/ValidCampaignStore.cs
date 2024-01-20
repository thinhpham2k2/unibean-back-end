using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidCampaignStore : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid campaign stores";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            List<string> storeIds = create.CampaignStores.Select(c => c.StoreId).ToList();
            if (storeIds.Count.Equals(storeIds.Distinct().ToList().Count) && storeIds.Count > 0)
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
