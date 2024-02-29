using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Admins;

public class UpdateAdminModel
{
    [Required(ErrorMessage = "Tên đầy đủ là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài của họ tên từ 3 đến 255 ký tự")]
    public string FullName { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile Avatar { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
