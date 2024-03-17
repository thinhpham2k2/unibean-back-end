using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidProduct : ValidationAttribute
{
    private new const string ErrorMessage = "Sản phẩm không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var productRepo = validationContext.GetService<IProductRepository>();
        var pro = productRepo.GetById(value.ToString());
        if (pro != null && (bool)pro.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
