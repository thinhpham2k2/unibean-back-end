using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;
using Unibean.Service.Models.Validations;

namespace Unibean.Service.Validations;

public class ValidTotalIncome : ValidationAttribute
{
    private new const string ErrorMessage = "Chi phí không hợp lệ";

    private const string ErrorMessage1 = "Số dư ví xanh của thương hiệu là không đủ";

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var brandRepo = validationContext.GetService<IBrandRepository>();
        var voucherRepo = validationContext.GetService<IVoucherRepository>();
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.CampaignDetails != null)
            {
                if (decimal.TryParse(value.ToString(), out decimal amount))
                {
                    if (amount.Equals(create.CampaignDetails.Select(v
                        =>
                    {
                        var voucher = voucherRepo.GetById(v.VoucherId);
                        return voucher != null && (bool)voucher.State ? v.Quantity * voucher.Price * voucher.Rate : 0;
                    }).Sum()))
                    {
                        var brand = brandRepo.GetById(create.BrandId);
                        if (brand != null && (bool)brand.State)
                        {
                            if (amount <= brand.Wallets.Select(w => w.Balance).Sum())
                            {
                                return ValidationResult.Success;
                            }
                            return new ValidationResult(ErrorMessage1);
                        }
                    }
                }
            }
        }
        else if (validationContext.ObjectInstance is CampaignCDModel verify)
        {
            if (verify.CampaignDetails != null)
            {
                if (decimal.TryParse(value.ToString(), out decimal amount))
                {
                    if (amount.Equals(verify.CampaignDetails.Select(v
                        =>
                    {
                        var voucher = voucherRepo.GetById(v.VoucherId);
                        return voucher != null && (bool)voucher.State ? v.Quantity * voucher.Price * voucher.Rate : 0;
                    }).Sum()))
                    {
                        var brand = brandRepo.GetById(verify.BrandId);
                        if (brand != null && (bool)brand.State)
                        {
                            if (amount <= brand.Wallets.Select(w => w.Balance).Sum())
                            {
                                return ValidationResult.Success;
                            }
                            return new ValidationResult(ErrorMessage1);
                        }
                    }
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
