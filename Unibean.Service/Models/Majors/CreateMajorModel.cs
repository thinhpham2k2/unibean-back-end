using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Majors;

public class CreateMajorModel
{
    [Required(ErrorMessage = "Tên chuyên ngành là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên chuyên ngành từ 3 đến 255 ký tự")]
    public string MajorName { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
