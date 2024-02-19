namespace Unibean.Service.Models.Majors;

public class MajorExtraModel
{
    public string Id { get; set; }
    public string MajorName { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public int? NumberOfStudents { get; set; }
    public int? NumberOfCampaigns { get; set; }
}
