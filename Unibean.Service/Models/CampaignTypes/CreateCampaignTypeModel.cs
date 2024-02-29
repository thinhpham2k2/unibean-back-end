using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.CampaignTypes;

public class CreateCampaignTypeModel
{
    [Required(ErrorMessage = "Tên loại là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Tên loại có độ dài từ 3 đến 255 ký tự")]
    public string TypeName { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
