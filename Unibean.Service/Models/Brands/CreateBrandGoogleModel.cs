using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Brands;

public class CreateBrandGoogleModel
{
    [ValidAccount]
    [Required(ErrorMessage = "Tài khoản là bắt buộc")]
    public string AccountId { get; set; }

    [Required(ErrorMessage = "Tên thương hiệu là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên thương hiệu từ 3 đến 255 ký tự")]
    public string BrandName { get; set; }

    [ValidEmail]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Required(ErrorMessage = "Email là bắt buộc")]
    public string Email { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
