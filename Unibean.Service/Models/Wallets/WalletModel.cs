namespace Unibean.Service.Models.Wallets;

public class WalletModel
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string TypeId { get; set; }
    public string TypeName { get; set; }
    public decimal? Balance { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
