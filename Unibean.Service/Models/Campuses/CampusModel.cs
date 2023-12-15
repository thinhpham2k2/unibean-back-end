namespace Unibean.Service.Models.Campuses;

public class CampusModel
{
    public string Id { get; set; }
    public string UniversityId { get; set; }
    public string UniversityName { get; set; }
    public string AreaId { get; set; }
    public string AreaName { get; set; }
    public string CampusName { get; set; }
    public TimeOnly? OpeningHours { get; set; }
    public TimeOnly? ClosingHours { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Link { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public string Description { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
