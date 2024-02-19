using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Students;

public class UpdateStudentModel
{
    [Required(ErrorMessage = "Tên đầy đủ là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài của họ tên từ 3 đến 255 ký tự")]
    public string FullName { get; set; }

    [ValidMajor]
    [Required(ErrorMessage = "Chuyên ngành là bắt buộc")]
    public string MajorId { get; set; }

    [ValidCampus]
    [Required(ErrorMessage = "Cơ sở là bắt buộc")]
    public string CampusId { get; set; }

    /// <summary>
    /// Nữ = 1, Nam = 2
    /// </summary>
    [ValidGender]
    [Required(ErrorMessage = "Giới tính là bắt buộc")]
    public int? Gender { get; set; }

    public IFormFile Avatar { get; set; }

    public string Address { get; set; }
}
