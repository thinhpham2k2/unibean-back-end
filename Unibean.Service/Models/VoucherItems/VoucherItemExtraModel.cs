﻿namespace Unibean.Service.Models.VoucherItems;

public class VoucherItemExtraModel
{
    public string Id { get; set; }
    public string VoucherId { get; set; }
    public string VoucherName { get; set; }
    public string VoucherImage { get; set; }
    public string TypeId { get; set; }
    public string TypeName { get; set; }
    public string TypeImage { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string BrandImage { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string CampaignImage { get; set; }
    public string UsedAt { get; set; }
    public string VoucherCode { get; set; }
    public decimal? Price { get; set; }
    public decimal? Rate { get; set; }
    public bool? IsBought { get; set; }
    public bool? IsUsed { get; set; }
    public DateOnly? ValidOn { get; set; }
    public DateOnly? ExpireOn { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateBought { get; set; }
    public DateTime? DateUsed { get; set; }
    public string Condition { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}