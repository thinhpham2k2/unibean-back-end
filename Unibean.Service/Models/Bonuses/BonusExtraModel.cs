using Unibean.Service.Models.BonusTransactions;

namespace Unibean.Service.Models.Bonuses;

public class BonusExtraModel
{
    public string Id { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string BrandAcronym { get; set; }
    public string BrandLogo { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string StoreImage { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string StudentAvatar { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public virtual ICollection<BonusTransactionModel> BonusTransactions { get; set; }
}
