using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidVoucherType : ValidationAttribute
{
    private new const string ErrorMessage = "Loại khuyến mãi không hợp lệ";

    private readonly IVoucherTypeRepository voucherTypeRepo = new VoucherTypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var type = voucherTypeRepo.GetById(value.ToString());
        if (type != null && (bool)type.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
