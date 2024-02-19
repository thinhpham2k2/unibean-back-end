using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Accounts;

public class CreateGoogleAccountModel
{
    [ValidRole]
    public int Role { get; set; }

    [ValidEmail]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Required(ErrorMessage = "Email là bắt buộc")]
    [StringLength(320, MinimumLength = 3,
            ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }

    [Required(ErrorMessage = "IsVerify là bắt buộc")]
    public bool? IsVerify { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
