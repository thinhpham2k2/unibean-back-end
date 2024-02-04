using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidStudent : ValidationAttribute
{
    private readonly StudentState[] states;

    public ValidStudent(StudentState[] states)
    {
        this.states = states;
    }

    private new const string ErrorMessage = "Sinh viên không hợp lệ";

    private readonly IStudentRepository studentRepo = new StudentRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var student = studentRepo.GetById(value.ToString());
        if (student != null && states.Contains(student.State.Value))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
