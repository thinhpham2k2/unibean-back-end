using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidState : ValidationAttribute
{
    private new const string ErrorMessage = "Trạng thái đơn hàng không hợp lệ"; 
    
    private readonly IStateRepository stateRepo = new StateRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var state = stateRepo.GetById(value.ToString());
        if (state != null && (bool)state.States)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
