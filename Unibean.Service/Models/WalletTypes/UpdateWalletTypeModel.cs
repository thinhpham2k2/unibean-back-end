﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Unibean.Service.Models.WalletTypes;

public class UpdateWalletTypeModel
{
    [Required(ErrorMessage = "Tên loại ví là bắt buộc")]
    [StringLength(255, MinimumLength = 3,
            ErrorMessage = "Tên loại ví có độ dài từ 3 đến 255 ký tự")]
    public string TypeName { get; set; }

    public IFormFile Image { get; set; }

    public string Description { get; set; }

    [Required(ErrorMessage = "Trạng thái là bắt buộc")]
    public bool? State { get; set; }
}
