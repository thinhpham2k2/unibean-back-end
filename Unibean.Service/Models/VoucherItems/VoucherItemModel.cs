namespace Unibean.Service.Models.VoucherItems;

public class VoucherItemModel
{
    public string Id { get; set; }
    public string VoucherId { get; set; }
    public string VoucherName { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public decimal? Price { get; set; }
    public decimal? Rate { get; set; }
    public bool? IsBought { get; set; }
    public bool? IsUsed { get; set; }
    public DateOnly? ValidOn { get; set; }
    public DateOnly? ExpireOn { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
