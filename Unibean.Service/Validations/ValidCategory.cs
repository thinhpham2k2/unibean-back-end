using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidCategory : ValidationAttribute
{
    private new const string ErrorMessage = "Loại sản phẩm không hợp lệ"; 
    
    private readonly ICategoryRepository categoryRepo = new CategoryRepository(); 
    
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var cate = categoryRepo.GetById(value.ToString());
        if (cate != null && (bool)cate.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
