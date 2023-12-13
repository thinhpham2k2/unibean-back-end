namespace Unibean.Service.Models.CampaignMajors;

public class CampaignMajorModel
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string MajorId { get; set; }
    public string MajorName { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
