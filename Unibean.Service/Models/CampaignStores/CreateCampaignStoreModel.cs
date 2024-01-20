using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignStores;

public class CreateCampaignStoreModel
{
    [ValidStore]
    [Required(ErrorMessage = "Store is required")]
    public string StoreId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "State is required")]
    public bool? State { get; set; }
}
