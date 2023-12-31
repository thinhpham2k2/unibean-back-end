using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidChallengeType : ValidationAttribute
{
    private readonly IChallengeTypeRepository challengeTypeRepo = new ChallengeTypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (challengeTypeRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid challenge type");
    }
}
