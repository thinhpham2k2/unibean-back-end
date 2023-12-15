namespace Unibean.Service.Models.Vouchers;

public class VoucherModel
{
    public string Id { get; set; }
    public string PartnerId { get; set; }
    public string BrandName { get; set; }
    public string VoucherName { get; set; }
    public decimal? Price { get; set; }
    public decimal? Rate { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
