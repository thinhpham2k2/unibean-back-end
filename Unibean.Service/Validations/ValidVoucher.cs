using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidVoucher : ValidationAttribute
{
    private new const string ErrorMessage = "Khuyến mãi không hợp lệ";

    private readonly IVoucherRepository voucherRepo = new VoucherRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var vou = voucherRepo.GetById(value.ToString());
        if (vou != null && (bool)vou.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
