using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.VoucherItems;

public class InsertVoucherItemModel
{
    [ValidVoucher]
    [Required(ErrorMessage = "Khuyến mãi là bắt buộc")]
    public string VoucherId { get; set; }

    [ValidExtension(new[] { ".xlsx" })]
    [Required(ErrorMessage = "Mẫu nhập khuyến mãi là bắt buộc")]
    public IFormFile Template { get; set; }

    public string Description { get; set; }
}
