namespace Unibean.Service.Models.RequestTransactions;

public class RequestTransactionModel
{
    public string Id { get; set; }
    public string WalletId { get; set; }
    public string RequestId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
