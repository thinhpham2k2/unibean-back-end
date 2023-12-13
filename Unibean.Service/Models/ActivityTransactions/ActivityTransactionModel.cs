namespace Unibean.Service.Models.ActivityTransactions;

public class ActivityTransactionModel
{
    public string Id { get; set; }
    public string ActivityId { get; set; }
    public string WalletId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
