using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Accounts;

public class CreateBrandAccountModel
{
    [Required(ErrorMessage = "Tên thương hiệu là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên thương hiệu từ 3 đến 255 ký tự")]
    public string BrandName { get; set; }

    public string Acronym { get; set; }

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

    [ValidPhone]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Mã băm là bắt buộc")]
    public string HashCode { get; set; }

    [ValidVerificationCode(nameof(HashCode))]
    [Required(ErrorMessage = "Yêu cầu mã xác nhận")]
    public string VerificationCode { get; set; }

    [ValidEmail]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Required(ErrorMessage = "Email là bắt buộc")]
    public string Email { get; set; }

    public string Address { get; set; }

    public IFormFile Logo { get; set; }

    public IFormFile CoverPhoto { get; set; }

    public string Link { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
