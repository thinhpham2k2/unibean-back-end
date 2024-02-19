using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;

namespace Unibean.Service.Validations;

public class ValidItem : ValidationAttribute
{
    private new const string ErrorMessage = "Khuyến mãi không hợp lệ";

    private readonly IVoucherItemRepository voucherItemRepo = new VoucherItemRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var item = voucherItemRepo.GetById(value.ToString());
        if (item != null && (bool)item.State)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult(ErrorMessage);
    }
}
