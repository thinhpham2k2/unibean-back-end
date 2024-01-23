using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Campaigns;

namespace Unibean.Service.Validations;

public class ValidTotalIncome : ValidationAttribute
{
    private new const string ErrorMessage = "Invalid cost";

    private const string ErrorMessage1 = "The balance of the brand's green and red wallets is insufficient";

    private readonly IBrandRepository brandRepo = new BrandRepository();

    private readonly IVoucherRepository voucherRepo = new VoucherRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (validationContext.ObjectInstance is CreateCampaignModel create)
        {
            if (create.Vouchers != null)
            {
                if (decimal.TryParse(value.ToString(), out decimal amount))
                {
                    if (amount.Equals(create.Vouchers.Select(v
                        =>
                    {
                        var voucher = voucherRepo.GetById(v.VoucherId);
                        return voucher != null ? v.Quantity * voucher.Price * voucher.Rate : 0;
                    }).Sum()))
                    {
                        var brand = brandRepo.GetById(create.BrandId);
                        if (brand != null)
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
