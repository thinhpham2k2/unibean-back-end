using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Students;

public class UpdateStudentVerifyModel
{
    [Required(ErrorMessage = "Cần có ảnh mặt trước thẻ sinh viên")]
    public IFormFile StudentCardFront { get; set; }

    [Required(ErrorMessage = "Cần có ảnh mặt sau thẻ sinh viên")]
    public IFormFile StudentCardBack { get; set; }

    [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Độ dài mã sinh viên từ 3 đến 50 ký tự")]
    public string Code { get; set; }
}
