using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Campaigns;

public class UpdateCampaignModel
{
    [ValidCampaignType]
    [Required(ErrorMessage = "Loại chiến dịch là bắt buộc")]
    public string TypeId { get; set; }

    [Required(ErrorMessage = "Tên chiến dịch là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên chiến dịch từ 3 đến 255 ký tự")]
    public string CampaignName { get; set; }

    public IFormFile Image { get; set; }

    [Required(ErrorMessage = "Điều kiện là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài của điều kiện phải từ 3 ký tự trở lên")]
    public string Condition { get; set; }

    public string Link { get; set; }

    [Required(ErrorMessage = "Mô tả là bắt buộc")]
    [StringLength(int.MaxValue, MinimumLength = 3,
            ErrorMessage = "Độ dài mô tả phải từ 3 ký tự trở lên")]
    public string Description { get; set; }
}
