using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidChallengeType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại thử thách không hợp lệ"; 
    
    private readonly IChallengeTypeRepository challengeTypeRepo = new ChallengeTypeRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var type = challengeTypeRepo.GetById(value.ToString());
        if (type != null && (bool)type.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
