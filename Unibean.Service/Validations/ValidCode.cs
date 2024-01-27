using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCode : ValidationAttribute
{
    private new const string ErrorMessage = "Mã sinh viên đã được sử dụng";

    private readonly IStudentRepository studentRepo = new StudentRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string code = value.ToString();
        if (studentRepo.CheckCodeDuplicate(code))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
