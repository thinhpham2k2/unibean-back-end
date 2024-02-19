namespace Unibean.Service.Models.VoucherTypes;

public class VoucherTypeExtraModel
{
    public string Id { get; set; }
    public string TypeName { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public int? NumberOfVouchers { get; set; }
}
