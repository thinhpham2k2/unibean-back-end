using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Stations;

public class UpdateStationModel
{
    [Required(ErrorMessage = "Tên trạm là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên trạm từ 3 đến 255 ký tự")]
    public string StationName { get; set; }

    [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
    public string Address { get; set; }

    public IFormFile Image { get; set; }

    public TimeOnly? OpeningHours { get; set; }

    public TimeOnly? ClosingHours { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    public string Phone { get; set; }

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; }

    public string Description { get; set; }

    /// <summary>
    /// Active = 1, Inactive = 2, Closed = 3
    /// </summary>
    [ValidStationState]
    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public int? State { get; set; }
}
