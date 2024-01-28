using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Stores;

public class UpdateStoreModel
{
    [ValidArea]
    [Required(ErrorMessage = "Khu vực là bắt buộc")]
    public string AreaId { get; set; }

    [Required(ErrorMessage = "Tên cửa hàng là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên cửa hàng từ 3 đến 255 ký tự")]
    public string StoreName { get; set; }

    [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài của địa chỉ từ 3 đến 255 ký tự")]
    public string Address { get; set; }

    public IFormFile Avatar { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
