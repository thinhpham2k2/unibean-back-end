using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCode : ValidationAttribute
{
    private readonly IStudentRepository studentRepo = new StudentRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string code = value.ToString();
        if (studentRepo.CheckCodeDuplicate(code))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Student code is already in use");
    }
}
