using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Students;

public class UpdateStudentIdModel
{
    [ValidMajor]
    [Required(ErrorMessage = "Chuyên ngành là bắt buộc")]
    public string MajorId { get; set; }

    [ValidCampus]
    [Required(ErrorMessage = "Cơ sở là bắt buộc")]
    public string CampusId { get; set; }

    [Required(ErrorMessage = "Cần có ảnh mặt trước thẻ sinh viên")]
    public IFormFile StudentCardFront { get; set; }

    [Required(ErrorMessage = "Cần có ảnh mặt sau thẻ sinh viên")]
    public IFormFile StudentCardBack { get; set; }

    [ValidCode]
    [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Độ dài mã sinh viên từ 3 đến 50 ký tự")]
    public string Code { get; set; }
}
