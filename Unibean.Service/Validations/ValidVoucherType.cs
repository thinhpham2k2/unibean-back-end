using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidVoucherType : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid voucher type";

    private readonly IVoucherTypeRepository voucherTypeRepo = new VoucherTypeRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (voucherTypeRepo.GetById(value.ToString()) != null)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
