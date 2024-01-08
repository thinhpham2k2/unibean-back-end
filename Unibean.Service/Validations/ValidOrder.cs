using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidOrder : ValidationAttribute
{
    private readonly IOrderRepository orderRepo = new OrderRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (orderRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult("Invalid order");
    }
}
