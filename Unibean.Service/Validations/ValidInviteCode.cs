using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace Unibean.Service.Validations;

public class ValidInviteCode : ValidationAttribute
{
    private readonly IStudentRepository studentRepo = new StudentRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        string inviteCode = value?.ToString();
        if (inviteCode.IsNullOrEmpty() || studentRepo.CheckInviteCode(inviteCode))
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid invite code");
    }
}
