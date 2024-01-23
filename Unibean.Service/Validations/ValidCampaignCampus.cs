using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidCampaignCampus : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid campaign campuses";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.CampaignCampuses != null)
            {
                List<string> campusIds = create.CampaignCampuses.Select(c => c.CampusId).ToList();
                if (campusIds.Count.Equals(campusIds.Distinct().ToList().Count) && campusIds.Count > 0)
                {
                    return ValidationResult.Success;
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
