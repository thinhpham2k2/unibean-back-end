namespace Unibean.Service.Models.WalletTransactions;

public class WalletTransactionModel
{
    public string Id { get; set; }
    public string WalletId { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
