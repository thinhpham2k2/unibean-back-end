using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidStudent : ValidationAttribute
{
    private new const string ErrorMessage = "Sinh viên không hợp lệ";

    private readonly IStudentRepository studentRepo = new StudentRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var student = studentRepo.GetById(value.ToString());
        if (student != null && (bool)student.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
