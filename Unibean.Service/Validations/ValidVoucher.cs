using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidVoucher : ValidationAttribute
{
    private new const string ErrorMessage = "Khuyến mãi không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var voucherRepo = validationContext.GetService<IVoucherRepository>();
        var vou = voucherRepo.GetById(value.ToString());
        if (vou != null && (bool)vou.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
