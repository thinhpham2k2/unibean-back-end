using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.Roles;

public class UpdateRoleModel
{
    [Required(ErrorMessage = "Tên vai trò là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Tên vai trò có độ dài từ 3 đến 255 ký tự")]
    public string RoleName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
