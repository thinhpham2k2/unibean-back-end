using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Service.Models.Orders;
using Unibean.Repository.Entities;

namespace Unibean.Service.Validations;

public class ValidDetail : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid order";

    private const string ErrorMessage1 = "You are not level enough to exchange for this product";

    private readonly IProductRepository productRepo = new ProductRepository();

    private readonly IStudentRepository studentRepo = new StudentRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateOrderModel create)
        {

            decimal condition = (decimal)studentRepo.GetById(create.StudentId).Level.Condition;
            foreach (var detail in create.OrderDetails)
            {
                Product product = productRepo.GetById(detail.ProductId);
                if(product != null)
                {
                    if (product.Level.Condition > condition)
                    {
                        return new ValidationResult(ErrorMessage1);
                    }
                }
                else
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
