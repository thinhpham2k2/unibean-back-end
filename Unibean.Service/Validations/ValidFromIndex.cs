﻿using System.ComponentModel.DataAnnotations;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Service.Models.CampaignDetails;
using Microsoft.IdentityModel.Tokens;

namespace Unibean.Service.Validations;

public class ValidFromIndex : ValidationAttribute
{
    private new const string ErrorMessage = "Chỉ mục không hợp lệ";

    private readonly IVoucherRepository voucherRepo = new VoucherRepository();

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (int.TryParse(value.ToString(), out int fromIndex))
        {
            if (validationContext.ObjectInstance is CreateCampaignDetailModel create)
            {
                Voucher voucher = voucherRepo.GetById(create.VoucherId);
                if (voucher != null && (bool)voucher.State)
                {
                    if (voucher.VoucherItems.Where(
                        i => (bool)i.State && (bool)i.Status
                        && !(bool)i.IsLocked && !(bool)i.IsBought 
                        && (fromIndex.Equals(0) || i.Index.Equals(fromIndex))
                        && !(bool)i.IsUsed && i.CampaignDetailId.IsNullOrEmpty()).FirstOrDefault() != null)
                    {
                        return ValidationResult.Success;
                    }
                    return new ValidationResult("Chỉ mục của khuyến mãi " + voucher.VoucherName + " không hợp lệ");
                }
            }
        }
        return new ValidationResult(ErrorMessage);
    }
}
