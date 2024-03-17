using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.OrderDetails;

namespace Unibean.Service.Validations;

public class ValidQuantity : ValidationAttribute
{
    private new const string ErrorMessage = "Số lượng không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var productRepo = validationContext.GetService<IProductRepository>();
        if (int.TryParse(value.ToString(), out int quantity))
        {
            if (validationContext.ObjectInstance is CreateDetailModel create)
            {
                Product product = productRepo.GetById(create.ProductId);
                if (product != null && (bool)product.State)
                {
                    if (product.Quantity >= quantity)
                    {
                        return ValidationResult.Success;
                    }
                    return new ValidationResult("Số lượng của " + product.ProductName + " không đủ");
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
