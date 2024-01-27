using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.OrderDetails;
using Unibean.Service.Models.Orders;

namespace Unibean.Service.Validations;

public class ValidAmount : ValidationAttribute
{
    private new const string ErrorMessage = "Số tiền không hợp lệ";

    private readonly IProductRepository productRepo = new ProductRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateOrderModel create)
        {
            List<CreateDetailModel> detailList = create.OrderDetails.ToList();

            if (decimal.TryParse(value.ToString(), out decimal amount))
            {
                foreach (CreateDetailModel detail in detailList)
                {
                    Product product = productRepo.GetById(detail.ProductId);
                    if (product != null && (bool)product.State)
                    {
                        amount -= (decimal)(product.Price * detail.Quantity);
                    }
                    else
                    {
                        return new ValidationResult(ErrorMessage);
                    }
                }

                if (amount.Equals(0))
                {
                    return ValidationResult.Success;
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
