namespace Unibean.Service.Models.CampaignDetails;

public class CampaignDetailModel
{
    public string Id { get; set; }
    public string VoucherId { get; set; }
    public string VoucherName { get; set; }
    public string VoucherImage { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public decimal? Price { get; set; }
    public decimal? Rate { get; set; }
    public int? Quantity { get; set; }
    public int? FromIndex { get; set; }
    public int? ToIndex { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public int? QuantityInStock { get; set; }
    public int? QuantityInBought { get; set; }
    public int? QuantityInUsed { get; set; }
}
