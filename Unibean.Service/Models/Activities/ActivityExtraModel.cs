namespace Unibean.Service.Models.Activities;

public class ActivityExtraModel
{
    public string Id { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string BrandImage { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string StoreImage { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string CampaignImage { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentImage { get; set; }
    public decimal Amount { get; set; }
    public int WalletTypeId { get; set; }
    public string WalletType { get; set; }
    public string WalletTypeName { get; set; }
    public int TypeId { get; set; }
    public string Type { get; set; }
    public string TypeName { get; set; }
    public string VoucherItemId { get; set; }
    public string VoucherName { get; set; }
    public string VoucherCode { get; set; }
    public string VoucherPrice { get; set; }
    public string VoucherRate { get; set; }
    public string VoucherImage { get; set; }
    public string Description { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
