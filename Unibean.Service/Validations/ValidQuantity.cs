using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Service.Models.OrderDetails;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidQuantity : ValidationAttribute
{
    private new const string ErrorMessage = "Số lượng không hợp lệ";

    private readonly IProductRepository productRepo = new ProductRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
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
                return new ValidationResult(ErrorMessage);
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
