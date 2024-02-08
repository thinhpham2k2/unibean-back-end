namespace Unibean.Service.Models.Areas;

public class AreaExtraModel
{
    public string Id { get; set; }
    public string AreaName { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public string Address { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public int? NumberOfCampuses { get; set; }
    public int? NumberOfStores { get; set; }
}
