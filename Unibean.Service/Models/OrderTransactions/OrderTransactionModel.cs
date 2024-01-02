namespace Unibean.Service.Models.OrderTransactions;

public class OrderTransactionModel
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string WalletId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
