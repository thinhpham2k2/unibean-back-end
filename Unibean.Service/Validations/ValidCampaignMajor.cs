using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Validations;

namespace Unibean.Service.Validations;

public class ValidCampaignMajor : ValidationAttribute
{
    private new const string ErrorMessage = "Danh sách chuyên ngành không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.CampaignMajors != null)
            {
                List<string> majorIds = create.CampaignMajors.Select(c => c.MajorId).ToList();
                if (majorIds.Count.Equals(majorIds.Distinct().ToList().Count) && majorIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        else if (validationContext.ObjectInstance is CampaignMSCModel verify)
        {
            if (verify.CampaignMajors != null)
            {
                List<string> majorIds = verify.CampaignMajors.Select(c => c.MajorId).ToList();
                if (majorIds.Count.Equals(majorIds.Distinct().ToList().Count) && majorIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
