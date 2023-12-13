namespace Unibean.Service.Models.CampaignGenders;

public class CampaignGenderModel
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string GenderId { get; set; }
    public string GenderName { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
