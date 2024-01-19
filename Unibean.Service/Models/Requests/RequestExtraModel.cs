using Unibean.Service.Models.RequestTransactions;

namespace Unibean.Service.Models.Requests;

public class RequestExtraModel
{
    public string Id { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string BrandAcronym { get; set; }
    public string BrandLogo { get; set; }
    public string AdminId { get; set; }
    public string AdminName { get; set; }
    public string AdminAvatar { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public virtual ICollection<RequestTransactionModel> RequestTransactions { get; set; }
}
