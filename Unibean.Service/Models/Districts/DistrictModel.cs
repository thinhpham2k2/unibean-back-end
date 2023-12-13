namespace Unibean.Service.Models.Districts;

public class DistrictModel
{
    public string Id { get; set; }
    public string CityId { get; set; }
    public string CityName { get; set; }
    public string DistrictName { get; set; }
    public string Image { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
