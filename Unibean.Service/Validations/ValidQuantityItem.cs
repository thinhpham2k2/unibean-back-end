using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.CampaignDetails;

namespace Unibean.Service.Validations;

public class ValidQuantityItem : ValidationAttribute
{
    private new const string ErrorMessage = "Số lượng khuyến mãi không hợp lệ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var voucherRepo = validationContext.GetService<IVoucherRepository>();
        if (int.TryParse(value.ToString(), out int quantity))
        {
            if (validationContext.ObjectInstance is CreateCampaignDetailModel create)
            {
                Voucher voucher = voucherRepo.GetById(create.VoucherId);
                if (voucher != null && (bool)voucher.State)
                {
                    if (voucher.VoucherItems.Where(
                        i => (bool)i.State && (bool)i.Status
                        && !(bool)i.IsLocked && !(bool)i.IsBought
                        && (create.FromIndex.Equals(0) || i.Index >= create.FromIndex)
                        && !(bool)i.IsUsed && i.CampaignDetailId.IsNullOrEmpty()).Count() >= quantity)
                    {
                        return ValidationResult.Success;
                    }
                    return new ValidationResult("Số lượng của khuyến mãi " + voucher.VoucherName + " không đủ");
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
