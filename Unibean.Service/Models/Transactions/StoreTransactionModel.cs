namespace Unibean.Service.Models.Transactions;

public class StoreTransactionModel
{
    public string Id { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string Activity { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public string WalletId { get; set; }
    public int WalletTypeId { get; set; }
    public string WalletType { get; set; }
    public string WalletTypeName { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
