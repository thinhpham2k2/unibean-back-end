using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidOrder : ValidationAttribute
{
    private new const string ErrorMessage = "Đơn hàng không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var orderRepo = validationContext.GetService<IOrderRepository>();
        var order = orderRepo.GetById(value.ToString());
        if (order != null && (bool)order.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
