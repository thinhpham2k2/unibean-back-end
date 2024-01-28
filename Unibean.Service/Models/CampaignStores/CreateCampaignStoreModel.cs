using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignStores;

[ModelBinder(typeof(MetadataValueModelBinder))]
public class CreateCampaignStoreModel
{
    [ValidStore]
    [Required(ErrorMessage = "Cửa hàng là bắt buộc")]
    public string StoreId { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
