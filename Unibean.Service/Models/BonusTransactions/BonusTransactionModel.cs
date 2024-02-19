namespace Unibean.Service.Models.BonusTransactions;

public class BonusTransactionModel
{
    public string Id { get; set; }
    public string WalletId { get; set; }
    public string WalletTypeId { get; set; }
    public string WalletType { get; set; }
    public string WalletTypeName { get; set; }
    public string BonusId { get; set; }
    public decimal? Amount { get; set; }
    public decimal? Rate { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
