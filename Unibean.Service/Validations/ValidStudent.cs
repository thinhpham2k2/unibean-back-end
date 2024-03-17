using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidStudent : ValidationAttribute
{
    private readonly StudentState[] states;

    public ValidStudent(StudentState[] states)
    {
        this.states = states;
    }

    private new const string ErrorMessage = "Sinh viên không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var studentRepo = validationContext.GetService<IStudentRepository>();
        var student = studentRepo.GetByIdForValidation(value.ToString());
        if (student != null && states.Contains(student.State.Value))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
