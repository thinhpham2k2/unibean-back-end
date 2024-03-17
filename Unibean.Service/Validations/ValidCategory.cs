using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidCategory : ValidationAttribute
{
    private new const string ErrorMessage = "Loại sản phẩm không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var categoryRepo = validationContext.GetService<ICategoryRepository>();
        var cate = categoryRepo.GetById(value.ToString());
        if (cate != null && (bool)cate.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
