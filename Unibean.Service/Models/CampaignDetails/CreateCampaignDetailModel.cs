using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignDetails;

[ModelBinder(typeof(MetadataValueModelBinder))]
public class CreateCampaignDetailModel
{
    [ValidVoucher]
    [Required(ErrorMessage = "Khuyến mãi là bắt buộc")]
    public string VoucherId { get; set; }

    [ValidQuantityItem]
    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Quantity { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
