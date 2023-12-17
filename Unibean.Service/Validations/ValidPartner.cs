using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidPartner : ValidationAttribute
{
    private readonly IPartnerRepository partnerRepo = new PartnerRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (partnerRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid partner");
    }
}
