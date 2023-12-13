namespace Unibean.Service.Models.Stores;

public class StoreModel
{
    public string Id { get; set; }
    public string PartnerId { get; set; }
    public string BrandName { get; set; }
    public string AreaId { get; set; }
    public string AreaName { get; set; }
    public string StoreName { get; set; }
    public string UserName { get; set; }
    public string Image { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public TimeOnly? OpeningHours { get; set; }
    public TimeOnly? ClosingHours { get; set; }
    public string Description { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
