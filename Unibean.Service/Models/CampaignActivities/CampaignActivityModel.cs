namespace Unibean.Service.Models.CampaignActivities;

public class CampaignActivityModel
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public int StateId { get; set; }
    public string State { get; set; }
    public string StateName { get; set; }
    public DateTime? DateCreated { get; set; }
    public string Description { get; set; }
    public bool? Status { get; set; }
}
