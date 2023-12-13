namespace Unibean.Service.Models.CampaignCampuses;

public class CampaignCampusModel
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string CampusId { get; set; }
    public string CampusName { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
