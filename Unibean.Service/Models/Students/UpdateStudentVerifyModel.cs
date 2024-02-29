using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Students;

public class UpdateStudentVerifyModel
{
    [Required(ErrorMessage = "Cần có ảnh mặt trước thẻ sinh viên")]
    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile StudentCardFront { get; set; }

    [Required(ErrorMessage = "Cần có ảnh mặt sau thẻ sinh viên")]
    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile StudentCardBack { get; set; }

    [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Độ dài mã sinh viên từ 3 đến 50 ký tự")]
    public string Code { get; set; }
}
