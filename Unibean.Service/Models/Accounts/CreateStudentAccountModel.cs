using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Accounts;

public class CreateStudentAccountModel
{
    [ValidUserName]
    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
    [StringLength(50, MinimumLength = 5,
        ErrorMessage = "Độ dài tên đăng nhập từ 5 đến 50 ký tự")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(255, MinimumLength = 8,
        ErrorMessage = "Độ dài mật khẩu từ 8 đến 255 ký tự")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Cần phải xác nhận mật khẩu")]
    [Compare(nameof(Password), ErrorMessage = "Xác nhận mật khẩu không phù hợp")]
    public string PasswordConfirmed { get; set; }

    [ValidMajor]
    [Required(ErrorMessage = "Chuyên ngành là bắt buộc")]
    public string MajorId { get; set; }

    [ValidCampus]
    [Required(ErrorMessage = "Cơ sở là bắt buộc")]
    public string CampusId { get; set; }

    [Required(ErrorMessage = "Tên đầy đủ là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài của họ tên từ 3 đến 255 ký tự")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "Cần có ảnh mặt trước thẻ sinh viên")]
    public IFormFile StudentCardFront { get; set; }

    [Required(ErrorMessage = "Cần có ảnh mặt sau thẻ sinh viên")]
    public IFormFile StudentCardBack { get; set; }

    [ValidCode]
    [Required(ErrorMessage = "Mã sinh viên là bắt buộc")]
    [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Độ dài mã sinh viên từ 3 đến 50 ký tự")]
    public string Code { get; set; }

    /// <summary>
    /// Nữ = 1, Nam = 2
    /// </summary>
    [ValidGender]
    [Required(ErrorMessage = "Giới tính là bắt buộc")]
    public int? Gender { get; set; }

    [ValidInviteCode]
    public string InviteCode { get; set; }

    [ValidEmail]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Required(ErrorMessage = "Email là bắt buộc")]
    public string Email { get; set; }

    [ValidBirthday]
    [Required(ErrorMessage = "Ngày sinh là bắt buộc")]
    public DateOnly? DateOfBirth { get; set; }

    [ValidPhone]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    public string Phone { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
