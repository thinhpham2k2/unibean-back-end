namespace Unibean.Service.Models.CampaignStores;

public class CampaignStoreModel
{
    public string Id { get; set; }
    public string CampaignId { get; set; }
    public string CampaignName { get; set; }
    public string StoreId { get; set; }
    public string StoreName { get; set; }
    public string Description { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
}
