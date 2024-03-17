using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Service.Validations;

public class ValidVoucherType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại khuyến mãi không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var voucherTypeRepo = validationContext.GetService<IVoucherTypeRepository>();
        var type = voucherTypeRepo.GetById(value.ToString());
        if (type != null && (bool)type.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
