using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Orders;

namespace Unibean.Service.Validations;

public class ValidDetail : ValidationAttribute
{
    private new const string ErrorMessage = "Chi tiết đơn hàng không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateOrderModel create)
        {
            List<string> productIds = create.OrderDetails.Select(c => c.ProductId).ToList();
            if (productIds.Count.Equals(productIds.Distinct().ToList().Count) && productIds.Count > 0)
            {
                return ValidationResult.Success;
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
