using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStudentChallenge : ValidationAttribute
{
    private readonly IStudentChallengeRepository studentChallengeRepo = new StudentChallengeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (studentChallengeRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid student challenge");
    }
}
