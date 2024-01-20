using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidTotalIncome : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid cost";

    private const string ErrorMessage1 = "The balance of the brand's green and red wallets is insufficient";

    private readonly IBrandRepository brandRepo = new BrandRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (decimal.TryParse(value.ToString(), out decimal amount))
            {
                var brand = brandRepo.GetById(create.BrandId);
                if (brand != null)
                {
                    if(amount <= brand.Wallets.Select(w => w.Balance).Sum())
                    {
                        return ValidationResult.Success;
                    }
                    return new ValidationResult(ErrorMessage1);
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
