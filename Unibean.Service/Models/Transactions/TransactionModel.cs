namespace Unibean.Service.Models.Transactions;

public class TransactionModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string RequestId { get; set; }
    public string WalletId { get; set; }
    public string WalletType { get; set; }
    public string WalletImage { get; set; }
    public string TypeName { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
