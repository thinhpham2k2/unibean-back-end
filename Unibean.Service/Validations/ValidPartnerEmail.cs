using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidPartnerEmail : ValidationAttribute
{
    private readonly IPartnerRepository partnerRepository = new PartnerRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string email = value.ToString();
        if (partnerRepository.CheckEmailDuplicate(email))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Email is already in use");
    }
}
