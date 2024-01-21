using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidConstraint : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid campaign campuses";

    private const string ErrorMessage1 = "Campus listings must be in the same area as store listings";

    private readonly IStoreRepository storeRepository = new StoreRepository();

    private readonly ICampusRepository campusRepository = new CampusRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.CampaignStores != null && create.CampaignCampuses != null)
            {
                List<string> storeArea = create.CampaignStores
                    .Select(c => storeRepository.GetById(c.StoreId)).Select(s => s.AreaId).ToList();

                List<string> campusArea = create.CampaignCampuses
                    .Select(c => campusRepository.GetById(c.CampusId)).Select(c => c.AreaId).ToList();

                if (campusArea.All(a => storeArea.Contains(a)))
                {
                    return ValidationResult.Success;
                }
                return new ValidationResult(ErrorMessage1);
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
