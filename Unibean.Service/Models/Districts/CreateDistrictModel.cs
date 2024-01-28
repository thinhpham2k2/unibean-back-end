using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Districts;

public class CreateDistrictModel
{
    [ValidCity]
    [Required(ErrorMessage = "Thành phố là bắt buộc")]
    public string CityId { get; set; }

    [Required(ErrorMessage = "Tên quận là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Tên quận có độ dài từ 3 đến 255 ký tự")]
    public string DistrictName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
