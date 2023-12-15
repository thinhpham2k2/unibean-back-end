namespace Unibean.Service.Models.Universities;

public class UniversityModel
{
    public string Id { get; set; }
    public string UniversityName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Link { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
