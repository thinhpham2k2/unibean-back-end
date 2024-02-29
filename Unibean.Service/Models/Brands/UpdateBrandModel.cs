using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Brands;

public class UpdateBrandModel
{
    [Required(ErrorMessage = "Tên thương hiệu là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên thương hiệu từ 3 đến 255 ký tự")]
    public string BrandName { get; set; }

    public string Acronym { get; set; }

    public string Address { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile Logo { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile CoverPhoto { get; set; }

    public string Link { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
