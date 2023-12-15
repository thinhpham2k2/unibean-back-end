namespace Unibean.Service.Models.Campaigns;

public class CampaignModel
{
    public string Id { get; set; }
    public string PartnerId { get; set; }
    public string BrandName { get; set; }
    public string Acronym { get; set; }
    public string TypeId { get; set; }
    public string TypeName { get; set; }
    public string CampaignName { get; set; }
    public string Image { get; set; }
    public string FileName { get; set; }
    public string Condition { get; set; }
    public string Link { get; set; }
    public DateOnly? StartOn { get; set; }
    public DateOnly? EndOn { get; set; }
    public int? Duration { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
