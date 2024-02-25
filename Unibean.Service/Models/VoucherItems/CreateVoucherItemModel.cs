using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Unibean.Service.Models.Parameters;
using Unibean.Service.Validations;

namespace Unibean.Service.Models.VoucherItems;

[ModelBinder(typeof(MetadataValueModelBinder))]
public class CreateVoucherItemModel
{
    [ValidVoucher]
    [Required(ErrorMessage = "Khuyến mãi là bắt buộc")]
    public string VoucherId { get; set; }

    [Required(ErrorMessage = "Số lượng là bắt buộc")]
    [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int? Quantity { get; set; }

    public string Description { get; set; }
}
