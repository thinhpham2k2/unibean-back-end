namespace Unibean.Service.Models.Requests;

public class RequestModel
{
    public string Id { get; set; }
    public string BrandId { get; set; }
    public string BrandName { get; set; }
    public string AdminId { get; set; }
    public string AdminName { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? States { get; set; }
    public bool? Status { get; set; }
}
