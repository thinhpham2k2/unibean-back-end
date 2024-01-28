using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStudentChallenge : ValidationAttribute
{
    private new const string ErrorMessage = "Thử thách không hợp lệ"; 
    
    private readonly IStudentChallengeRepository studentChallengeRepo = new StudentChallengeRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var chall = studentChallengeRepo.GetById(value.ToString());
        if (chall != null && (bool)chall.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
