﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.Areas;

public class UpdateAreaModel
{
    [ValidDistrict]
    [Required(ErrorMessage = "Quận là bắt buộc")]
    public string DistrictId { get; set; }

    [Required(ErrorMessage = "Tên khu vực là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Độ dài tên khu vực từ 3 đến 255 ký tự")]
    public string AreaName { get; set; }

    public IFormFile Image { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
