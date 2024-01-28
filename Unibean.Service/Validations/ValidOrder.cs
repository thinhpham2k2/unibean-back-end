using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidOrder : ValidationAttribute
{
    private new const string ErrorMessage = "Đơn hàng không hợp lệ";

    private readonly IOrderRepository orderRepo = new OrderRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var order = orderRepo.GetById(value.ToString());
        if (order != null && (bool)order.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
