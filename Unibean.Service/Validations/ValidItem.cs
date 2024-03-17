using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidItem : ValidationAttribute
{
    private new const string ErrorMessage = "Khuyến mãi không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var voucherItemRepo = validationContext.GetService<IVoucherItemRepository>();
        var item = voucherItemRepo.GetById(value.ToString());
        if (item != null && (bool)item.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
