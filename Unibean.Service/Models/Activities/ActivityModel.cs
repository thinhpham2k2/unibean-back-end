namespace Unibean.Service.Models.Activity;

public class ActivityModel
{
    public string Id { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string StudentId { get; set; }
    public string StudentName { get; set; }
    public string Type{ get; set; }
    public string VoucherItemId { get; set; }
    public string VoucherName { get; set; }
    public string Description { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
