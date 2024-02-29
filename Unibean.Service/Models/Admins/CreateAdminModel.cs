using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Admins;

public class CreateAdminModel
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

    [Required(ErrorMessage = "Tên đầy đủ là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài của họ tên từ 3 đến 255 ký tự")]
    public string FullName { get; set; }

    [ValidExtension(new[] { ".apng", ".avif", ".gif", ".jpg", ".jpeg", ".jfif", ".pjpeg", ".pjp", ".png", ".svg", ".webp" })]
    public IFormFile Avatar { get; set; }

    [ValidEmail]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Required(ErrorMessage = "Email là bắt buộc")]
    public string Email { get; set; }

    [ValidPhone]
    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
    public string Phone { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
