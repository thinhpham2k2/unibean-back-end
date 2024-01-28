using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidProduct : ValidationAttribute
{
    private new const string ErrorMessage = "Sản phẩm không hợp lệ"; 
    
    private readonly IProductRepository productRepo = new ProductRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var pro = productRepo.GetById(value.ToString());
        if (pro != null && (bool)pro.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
